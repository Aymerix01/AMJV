using System.Collections.Generic;
using UnityEngine;

class Follow : CharacterState
{
    private GameObject playerTarget;

    private GameObject[] path;

    private int etapeMvmtIA;
    private int playerPosInit;

    private int SetDestinationToFollowPlayer()
    {
        List<int> voisinsIndex = new List<int>();
        int randomVoisin;
        foreach (GameObject voisin in gridArray[playerTarget.GetComponent<CharacterStateController>().positionOfCharacter].GetComponent<GridStat>().voisins)
        {
            if (voisin != null && !voisin.GetComponent<GridStat>().hasEntityOnIt && !voisin.GetComponent<GridStat>().isDestinationForEntity)
            {
                voisinsIndex.Add(voisin.GetComponent<GridStat>().posInGridArray);
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
    public override CharacterState Enter(Transform characterT, int posCharacter, float s, float t, float r, float ra, GameObject[] g)
    {
        base.Enter(characterT, posCharacter, s, t, r, ra, g);
        playerTarget = GetPlayerTransform();
        if (playerTarget == null)
        {
            Debug.Log("PlayerTarget is Null");
            return Exit(new Idle());
        }
        playerPosInit = playerTarget.GetComponent<CharacterStateController>().positionOfCharacter;
        transform.gameObject.GetComponent<Animator>().SetBool("isWalking", true);
        int destination = SetDestinationToFollowPlayer();
        if (destination == -1)
        {
            Debug.Log("No Destination, escape");
            return Exit(new Escape());
        }
        path = FindPath.GetPathIA(transform, positionOfCharacter, destination, gridArray);
        etapeMvmtIA = path.Length - 1;
        return this;
    }

    public override CharacterState UpdateState()
    {
        base.UpdateState();
        if (IsIAarrivedEtape(0, path))
        {
            path[etapeMvmtIA].GetComponent<GridStat>().isDestinationForEntity = false;
            gridArray[positionOfCharacter].GetComponent<GridStat>().hasEntityOnIt = true;
            positionOfCharacter = path[0].GetComponent<GridStat>().posInGridArray;
            transform.gameObject.GetComponent<Animator>().SetBool("isWalking", false);
            if (playerPosInit != playerTarget.GetComponent<CharacterStateController>().positionOfCharacter)
            {
                return Exit(new Follow());
            }
           return Exit(new Attack());
        }
        else if (IsIAarrivedEtape(etapeMvmtIA, path))
        {
            path[etapeMvmtIA].GetComponent<GridStat>().hasEntityOnIt = false;
            etapeMvmtIA--;
            positionOfCharacter = path[etapeMvmtIA].GetComponent<GridStat>().posInGridArray;
            if(Vector3.Distance(playerTarget.transform.position, transform.position) < rangeToAttackPlayer)
            {
                transform.gameObject.GetComponent<Animator>().SetBool("isWalking", false);
                return Exit(new Attack());
            }
            return this;
        }
        else
        {
            path[etapeMvmtIA].GetComponent<GridStat>().hasEntityOnIt = true;
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
