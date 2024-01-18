using UnityEngine;

class Escape : CharacterState
{
    private GridStat[] path;

    private int etapeMvmtIA;

    private Camera camera;
    private Animator animator;
    private CharacterStateController characterStateController;

    private int ChooseDestinationClick()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            int layer = hit.collider.gameObject.layer;
            if (layer == 6) //layer 6 == platform
            {
                GameObject tile = hit.collider.gameObject;
                return tile.GetComponent<GridStat>().posInGridArray;
            }
        }
        return 0;
    }

    public override CharacterState Enter(Transform characterT, int posCharacter, float s, float t, float r, float ra, GridStat[] g)
    {
        base.Enter(characterT, posCharacter, s, t, r, ra, g);
        camera = Camera.main;
        animator = transform.GetComponent<Animator>();
        characterStateController = transform.GetComponent<CharacterStateController>();
        animator.SetBool("isWalking", false);
        int end;
        if (transform.gameObject.layer == 7)
        {
            end = ChooseDestinationClick();
        }
        else
        {
            end = ChooseDestinationRandom();
        }
        path = FindPath.GetPathIA(transform, positionOfCharacter, end, gridArray);
        etapeMvmtIA = path.Length - 1;
        return this;
    }

    public override CharacterState UpdateState()
    {
        base.UpdateState();
        if (characterStateController.pv <= 0)
        {
            return Exit(new Death());
        }
        if (IsIAarrivedEtape(0, path))
        {
            path[etapeMvmtIA].isDestinationForEntity = false;
            positionOfCharacter = path[0].posInGridArray;
            animator.SetBool("isWalking", false);
            return Exit(new Idle());
        }
        else if (IsIAarrivedEtape(etapeMvmtIA, path))
        {
            path[etapeMvmtIA].hasEntityOnIt = false;
            etapeMvmtIA--;
            positionOfCharacter = path[etapeMvmtIA].posInGridArray;
            return this;
        }
        else
        {
            path[etapeMvmtIA].hasEntityOnIt = true;
            animator.SetBool("isWalking", true);
            transform.position = Vector3.MoveTowards(transform.position,
                                            new Vector3(path[etapeMvmtIA].transform.position.x,
                                            0.34f + path[etapeMvmtIA].transform.position.y,
                                            path[etapeMvmtIA].transform.position.z),
                                            speed * Time.deltaTime);
            transform.LookAt(new Vector3(path[etapeMvmtIA].transform.position.x,
                                         transform.position.y,
                                         path[etapeMvmtIA].transform.position.z));
            return this;
        }
    }
}
