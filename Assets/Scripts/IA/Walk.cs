using System.Collections.Generic;
using UnityEngine;

class Walk : CharacterState
{
    private GameObject[] path;

    private int etapeMvmtIA;

    private int ChooseDestination()
    {
        int dest = Random.Range(0, gridArray.Length - 1);
        while (gridArray[dest] == null || gridArray[dest].GetComponent<PlatformeController>().hasEntityOnIt || 
            gridArray[dest].GetComponent<PlatformeController>().isDestinationForEntity ||
            dest == positionOfCharacter)
        {
            dest = Random.Range(0, gridArray.Length - 1);
        }
        return dest;
    }

    public override CharacterState Enter(Transform characterT, int posCharacter, float s, float t, float r, GameObject[] g)
    {
        base.Enter(characterT, posCharacter, s, t, r, g);
        int end = ChooseDestination();
        path = FindPath.GetPathIA(transform, positionOfCharacter, end, gridArray);
        etapeMvmtIA = path.Length - 1;
        return this;
    }

    public override CharacterState UpdateState()
    {
        base.UpdateState();
        if (SeePlayer())
        {
            foreach(GameObject p in path)
            {
                p.GetComponent<PlatformeController>().isDestinationForEntity = false;
                p.GetComponent<PlatformeController>().hasEntityOnIt = false;
            }
            return Exit(new Follow());
        }
        if (IsIAarrivedEtape(0, path))
        {
            path[etapeMvmtIA].GetComponent<PlatformeController>().isDestinationForEntity = false;
            positionOfCharacter = path[0].GetComponent<GridStat>().posInGridArray;
            transform.gameObject.GetComponent<Animator>().SetBool("isWalking", false);
            return Exit(new Idle());
        }
        else if (IsIAarrivedEtape(etapeMvmtIA, path))
        {
            path[etapeMvmtIA].GetComponent<PlatformeController>().hasEntityOnIt = false;
            positionOfCharacter = path[etapeMvmtIA].GetComponent<GridStat>().posInGridArray;
            etapeMvmtIA--;
            return this;
        }
        else
        {
            path[etapeMvmtIA].GetComponent<PlatformeController>().hasEntityOnIt = true;
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
