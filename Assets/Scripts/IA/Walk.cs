using System.Collections.Generic;
using UnityEngine;

class Walk : CharacterState
{
    private GridStat[] path;

    private int etapeMvmtIA;
    private List<GameObject> unitsSelected;

    private Animator animator;
    private CharacterStateController characterStateController;

    private int ChooseDestinationClickSingle()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            int layer = hit.collider.gameObject.layer;
            if (layer == 6) //layer 6 == platform
            {
                GameObject tile = hit.collider.gameObject;
                if (!tile.gameObject.GetComponent<GridStat>().hasEntityOnIt)
                {
                    return tile.GetComponent<GridStat>().posInGridArray;
                }
                else
                {
                    return positionOfCharacter;
                }
            }
        }
        return 0;
    }

    private int ChooseDestinationClickMultiple()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            int layer = hit.collider.gameObject.layer;
            if (layer == 6) //layer 6 == platform
            {
                GridStat tile = hit.collider.gameObject.GetComponent<GridStat>();
                GridStat[] voisins = tile.GetComponent<GridStat>().voisins;
                foreach (GameObject unit in unitsSelected) 
                {
                    int i = unitsSelected.IndexOf(transform.gameObject);
                    if (i > 6)
                    {
                        return positionOfCharacter;
                    }
                    if (i == 0)
                    {
                        return tile.posInGridArray;
                    }
                    else if (voisins[i-1] == null)
                    {
                        return positionOfCharacter;
                    }
                    else if (!voisins[i-1].hasEntityOnIt)
                    {
                        return voisins[i-1].posInGridArray;
                    }
                    else
                    {
                        return positionOfCharacter;
                    }
                }
          
            }
        }
        return 0;
    }

    public override CharacterState Enter(Transform characterT, int posCharacter, float s, float t, float r, float ra, GridStat[] g)
    {
        base.Enter(characterT, posCharacter, s, t, r, ra, g);
        unitsSelected = GameObject.FindWithTag("Game Manager").GetComponentInChildren<UnitSelections>().unitsSelected;
        animator = transform.GetComponent<Animator>();
        characterStateController = transform.GetComponent<CharacterStateController>();
        int end;
        if(transform.gameObject.layer == 7 && unitsSelected.Count == 1)
        {
            end = ChooseDestinationClickSingle();
        }
        else if (transform.gameObject.layer == 7 && unitsSelected.Count > 1)
        { 
            end = ChooseDestinationClickMultiple();
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
            path[0].isDestinationForEntity = false;
            path[etapeMvmtIA].hasEntityOnIt = false;
            path[etapeMvmtIA].nbrEntityOnIt -= 1;
            return Exit(new Death());
        }
        if (SeePlayer())
        {
            path[0].isDestinationForEntity = false;
            path[etapeMvmtIA].hasEntityOnIt = false;
            path[etapeMvmtIA].nbrEntityOnIt -= 1;
            return Exit(new Follow());
        }
        if (Input.GetMouseButtonDown(1) && unitsSelected.Contains(transform.gameObject))
        {
            path[0].isDestinationForEntity = false;
            path[etapeMvmtIA].hasEntityOnIt = false;
            path[etapeMvmtIA].nbrEntityOnIt -= 1;
            return Exit(new Walk());
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
            path[etapeMvmtIA].nbrEntityOnIt -= 1;
            etapeMvmtIA--;
            positionOfCharacter = path[etapeMvmtIA].posInGridArray;
            return this;
        }
        else
        {
            path[etapeMvmtIA].hasEntityOnIt = true;
            path[etapeMvmtIA].nbrEntityOnIt += 1;
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
