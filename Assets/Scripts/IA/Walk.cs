using System.Collections.Generic;
using UnityEngine;

class Walk : CharacterState
{
    private GameObject[] path;

    private int etapeMvmtIA;
    private List<GameObject> unitsSelected;

    private int ChooseDestinationClickSingle()
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
            //layer 6 == platform
            if (layer == 6)
            {
                GameObject tile = hit.collider.gameObject;
                GameObject[] voisins = tile.GetComponent<GridStat>().voisins;

                foreach (GameObject unit in unitsSelected) 
                {
                    int i = unitsSelected.IndexOf(transform.gameObject);
                    Debug.Log(i +" "+ unit);
                    //Debug.Log(voisins.Length);
                    if (i == 0)
                    {
                        return tile.GetComponent<GridStat>().posInGridArray;
                    }
                    else if (voisins[i] == null)
                    {
                        return positionOfCharacter;
                    }
                    else if (!voisins[i-1].GetComponent<GridStat>().hasEntityOnIt)
                    {
                        return voisins[i - 1].GetComponent<GridStat>().posInGridArray;
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

    public override CharacterState Enter(Transform characterT, int posCharacter, float s, float t, float r, float ra, GameObject[] g)
    {

        base.Enter(characterT, posCharacter, s, t, r, ra, g);
        unitsSelected = GameObject.FindWithTag("Game Manager").GetComponentInChildren<UnitSelections>().unitsSelected;
        int end = 0 ;
        if(transform.gameObject.layer == 7 && unitsSelected.Count == 1)
        {
            end = ChooseDestinationClickSingle();
        }
        else if (transform.gameObject.layer == 7 && unitsSelected.Count > 1) //&& unitsSelected.Count < 7)
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
        if (transform.gameObject.GetComponent<CharacterStateController>().pv <= 0)
        {
            return Exit(new Death());
        }
        if (SeePlayer())
        {
            foreach(GameObject p in path)
            {
                p.GetComponent<GridStat>().isDestinationForEntity = false;
                p.GetComponent<GridStat>().hasEntityOnIt = false;
            }
            return Exit(new Follow());
        }
        if (SeeEnemy())
        {
            GameObject[] enemiesSeen = GetEnemiesGameObject();
            foreach (GameObject enemy in enemiesSeen)
            {
                int childCount = enemy.transform.childCount;
                for (int i = 1; i < childCount; i++)
                {
                    enemy.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
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
