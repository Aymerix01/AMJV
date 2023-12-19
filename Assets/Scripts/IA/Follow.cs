using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

class Follow : CharacterState
{
    private GameObject playerTarget;

    private GameObject[] path;

    private int etapeMvmtIA;
    private GameObject GetPlayerTransform()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float closestPlayer = rangeToSeePlayer;
        GameObject target = null;
        foreach (GameObject player in players)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < closestPlayer)
            {
                closestPlayer = Vector3.Distance(player.transform.position, transform.position);
                target = player;
            }
        }
        return target;
    }
    private int SetDestinationToFollowPlayer()
    {
        List<int> voisinsIndex = new List<int>();
        int randomVoisin;
        foreach (GameObject voisin in gridArray[0].GetComponent<GridStat>().voisins)
        {
            /*
             0 = playerTarget.GetComponent<CharacterStateController>().positionOfCharacter 
             mais dépend du script du joueur donc je ne peux pas encore le faire !
             */
            if (!voisin.GetComponent<PlatformeController>().hasEntityOnIt && !voisin.GetComponent<PlatformeController>().isDestinationForEntity)
            {
                voisinsIndex.Add(voisin.GetComponent<GridStat>().posInGridArray);
            }
        }
        randomVoisin = Random.Range(0, voisinsIndex.Count-1);
        Debug.Log(randomVoisin);
        return voisinsIndex[randomVoisin];
    }
    public override CharacterState Enter(Transform characterT, int posCharacter, float s, float t, float r, GameObject[] g)
    {
        base.Enter(characterT, posCharacter, s, t, r, g);
        playerTarget = GetPlayerTransform();
        if (playerTarget == null )
        {
            return Exit(new Idle());
        }
        int destination = SetDestinationToFollowPlayer();
        path = FindPath.GetPathIA(transform, positionOfCharacter, destination, gridArray);
        etapeMvmtIA = path.Length - 1;
        return this;
    }

    public override CharacterState UpdateState()
    {
        base.UpdateState();
        if (IsIAarrivedEtape(0, path))
        {
            path[etapeMvmtIA].GetComponent<PlatformeController>().isDestinationForEntity = false;
            positionOfCharacter = path[0].GetComponent<GridStat>().posInGridArray;
            transform.gameObject.GetComponent<Animator>().SetBool("isWalking", false);
            return Exit(new Attack());
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
