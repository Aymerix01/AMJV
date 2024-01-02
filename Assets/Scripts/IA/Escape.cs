using System.Collections.Generic;
using UnityEngine;

class Escape : CharacterState
{
    private GameObject[] path;

    private int etapeMvmtIA;

    private int ChooseDestinationClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            int layer = hit.collider.gameObject.layer;
            //layer 6 == platform
            if (layer == 6)
            {
                //Obtention de la position sur la tilemap
                GameObject tile = hit.collider.gameObject;
                return tile.GetComponent<GridStat>().posInGridArray;
            }
        }
        return 0;
    }
    public override CharacterState Enter(Transform characterT, int posCharacter, float s, float t, float r, float ra, GameObject[] g)
    {

        base.Enter(characterT, posCharacter, s, t, r, ra, g);
        int end;
        transform.gameObject.GetComponent<Animator>().SetBool("isWalking", false);
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
        if (IsIAarrivedEtape(0, path))
        {
            path[etapeMvmtIA].GetComponent<GridStat>().isDestinationForEntity = false;
            positionOfCharacter = path[0].GetComponent<GridStat>().posInGridArray;
            transform.gameObject.GetComponent<Animator>().SetBool("isWalking", false);
            return Exit(new Idle());
        }
        else if (IsIAarrivedEtape(etapeMvmtIA, path))
        {
            path[etapeMvmtIA].GetComponent<GridStat>().hasEntityOnIt = false;
            etapeMvmtIA--;
            positionOfCharacter = path[etapeMvmtIA].GetComponent<GridStat>().posInGridArray;
            return this;
        }
        else
        {
            path[etapeMvmtIA].GetComponent<GridStat>().hasEntityOnIt = true;
            transform.gameObject.GetComponent<Animator>().SetBool("isWalking", true);
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
