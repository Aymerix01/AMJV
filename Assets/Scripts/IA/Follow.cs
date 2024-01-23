using System.Collections.Generic;
using UnityEngine;

class Follow : CharacterState
{
    private CharacterStateController characterTarget;

    private GridStat[] path;

    private List<GameObject> unitsSelected;

    private int etapeMvmtIA;
    private int characterPosInit;

    private bool oneTime = true;

    private Animator animator;
    private CharacterStateController characterStateController;
    private Camera camera;

    private int SetDestinationToFollowCharacter()
    {
        List<int> voisinsIndex = new List<int>();
        int randomVoisin;
        foreach (GridStat voisin in gridArray[characterTarget.positionOfCharacter].voisins)
        {
            if (voisin != null && !voisin.hasEntityOnIt && !voisin.isDestinationForEntity)
            {
                voisinsIndex.Add(voisin.posInGridArray);
            }
        }
        if (voisinsIndex.Count > 0)
        {
            randomVoisin = Random.Range(0, voisinsIndex.Count - 1);
            return voisinsIndex[randomVoisin];
        } else
        {
            return -1;
        }
    }
    public override CharacterState Enter(Transform characterT, int posCharacter, float s, float t, float r, float ra, GridStat[] g)
    {
        base.Enter(characterT, posCharacter, s, t, r, ra, g);
        unitsSelected = GameObject.FindWithTag("Game Manager").GetComponentInChildren<UnitSelections>().unitsSelected;
        animator = transform.GetComponent<Animator>();
        characterStateController = transform.GetComponent<CharacterStateController>();
        camera = Camera.main;
        if (transform.gameObject.tag == "Enemy")
        {
            characterTarget = GetPlayerTransform();
            if (characterTarget == null)
            {
                Debug.Log("PlayerTarget is Null");
                animator.SetBool("isWalking", false);
                return Exit(new Idle());
            }
            characterPosInit = characterTarget.positionOfCharacter;
            animator.SetBool("isWalking", true);
            int destination = SetDestinationToFollowCharacter();
            if (destination == -1)
            {
                Debug.Log("No Destination, escape");
                animator.SetBool("isWalking", false);
                return Exit(new Escape());
            }
            path = FindPath.GetPathIA(transform, positionOfCharacter, destination, gridArray);
            etapeMvmtIA = path.Length - 1;
            return this;
        }
        if (transform.gameObject.tag == "Player")
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    characterStateController.opponentToAttack = hit.collider.gameObject.GetComponent<CharacterStateController>();
                    characterTarget = hit.collider.gameObject.GetComponent<CharacterStateController>();
                }
            }
            if (characterTarget == null)
            {
                Debug.Log("PlayerTarget is Null");
                animator.SetBool("isWalking", false);
                return Exit(new Idle());
            }
            if (characterTarget.tag == "Enemy")
            {
                characterPosInit = characterTarget.positionOfCharacter;
                animator.SetBool("isWalking", true);
                int destination = SetDestinationToFollowCharacter();
                if (destination == -1)
                {
                    Debug.Log("No Destination, Selected");
                    animator.SetBool("isWalking", false);
                    return Exit(new Selected());
                }
                path = FindPath.GetPathIA(transform, positionOfCharacter, destination, gridArray);
                etapeMvmtIA = path.Length - 1;
            }
            return this;
        }
        return this;
    }
       
    public override CharacterState UpdateState()
    {
        base.UpdateState();
        if (characterStateController.pv <= 0)
        {
            path[0].isDestinationForEntity = false;
            path[etapeMvmtIA].hasEntityOnIt = false;
            path[etapeMvmtIA].nbrEntityOnIt -= 1;
            return Exit(new Death());
        }
        if (Input.GetMouseButtonDown(1) && unitsSelected.Contains(transform.gameObject))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                int layer = hit.collider.gameObject.layer;
                if (layer == 6 && !hit.collider.CompareTag("Hole") && !hit.collider.gameObject.GetComponent<GridStat>().hasEntityOnIt)
                {
                    return Exit(new Walk());
                }
                if (hit.collider.gameObject.tag == "Enemy")
                {
                    return Exit(new Follow());
                }
            }
        }
        if (IsIAarrivedEtape(0, path))
        {
            path[etapeMvmtIA].isDestinationForEntity = false;
            gridArray[positionOfCharacter].hasEntityOnIt = true;
            path[etapeMvmtIA].nbrEntityOnIt += 1;
            positionOfCharacter = path[0].posInGridArray;
            animator.SetBool("isWalking", false);
            if (characterPosInit != characterTarget.positionOfCharacter)
            {
                path[0].isDestinationForEntity = false;
                path[etapeMvmtIA].hasEntityOnIt = false;
                path[etapeMvmtIA].nbrEntityOnIt -= 1;
                return Exit(new Follow());
            }
            return Exit(new Attack());   
        }
        else if (IsIAarrivedEtape(etapeMvmtIA, path))
        {
            oneTime = true;
            if (Vector3.Distance(characterTarget.transform.position, transform.position) < rangeToAttackPlayer && path[etapeMvmtIA].nbrEntityOnIt==1)
            {
                path[0].isDestinationForEntity = false;
                animator.SetBool("isWalking", false);
                return Exit(new Attack());
            }
            path[etapeMvmtIA].hasEntityOnIt = false;
            path[etapeMvmtIA].nbrEntityOnIt -= 1;
            etapeMvmtIA--;
            positionOfCharacter = path[etapeMvmtIA].posInGridArray;
            return this;
        }
        else
        {
            path[etapeMvmtIA].hasEntityOnIt = true;
            if (oneTime)
            {
                path[etapeMvmtIA].nbrEntityOnIt += 1;
                oneTime = false;
            }
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
