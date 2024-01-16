using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

class Follow : CharacterState
{
    private GameObject characterTarget;

    private GameObject[] path;

    private int etapeMvmtIA;
    private int characterPosInit;

    private bool oneTime = true;

    private int SetDestinationToFollowCharacter()
    {
        List<int> voisinsIndex = new List<int>();
        int randomVoisin;
        foreach (GameObject voisin in gridArray[characterTarget.GetComponent<CharacterStateController>().positionOfCharacter].GetComponent<GridStat>().voisins)
        {
            if (voisin != null && !voisin.GetComponent<GridStat>().hasEntityOnIt && !voisin.GetComponent<GridStat>().isDestinationForEntity)
            {
                voisinsIndex.Add(voisin.GetComponent<GridStat>().posInGridArray);
                //Debug.Log(voisin.GetComponent<GridStat>().posInGridArray);
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

        if (transform.gameObject.tag == "Enemy")
        {
            characterTarget = GetPlayerTransform();

            if (characterTarget == null)
            {
                Debug.Log("PlayerTarget is Null");
                return Exit(new Idle());
            }

            characterPosInit = characterTarget.GetComponent<CharacterStateController>().positionOfCharacter;
            transform.gameObject.GetComponent<Animator>().SetBool("isWalking", true);
            int destination = SetDestinationToFollowCharacter();

            if (destination == -1)
            {
                Debug.Log("No Destination, escape");
                return Exit(new Escape());
            }
            path = FindPath.GetPathIA(transform, positionOfCharacter, destination, gridArray);
            etapeMvmtIA = path.Length - 1;
            return this;
        }

        if (transform.gameObject.tag == "Player")
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    transform.gameObject.GetComponent<CharacterStateController>().opponentToAttack = hit.collider.gameObject;
                    characterTarget = hit.collider.gameObject;
                }
            }
            
         
            if (characterTarget == null)
            {
                Debug.Log("PlayerTarget is Null");
                return Exit(new Idle());
            }

            if (characterTarget.tag == "Enemy")
            {
                characterPosInit = characterTarget.GetComponent<CharacterStateController>().positionOfCharacter;
                transform.gameObject.GetComponent<Animator>().SetBool("isWalking", true);

                int destination = SetDestinationToFollowCharacter();
                if (destination == -1)
                {
                    Debug.Log("No Destination, Selected");
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

        if (transform.gameObject.GetComponent<CharacterStateController>().pv <= 0)
        {
            path[0].GetComponent<GridStat>().isDestinationForEntity = false;
            path[etapeMvmtIA].GetComponent<GridStat>().hasEntityOnIt = false;
            path[etapeMvmtIA].GetComponent<GridStat>().nbrEntityOnIt -= 1;
            return Exit(new Death());
        }
        if (IsIAarrivedEtape(0, path))
        {
            path[etapeMvmtIA].GetComponent<GridStat>().isDestinationForEntity = false;
            gridArray[positionOfCharacter].GetComponent<GridStat>().hasEntityOnIt = true;
            path[etapeMvmtIA].GetComponent<GridStat>().nbrEntityOnIt += 1;
            positionOfCharacter = path[0].GetComponent<GridStat>().posInGridArray;
            transform.gameObject.GetComponent<Animator>().SetBool("isWalking", false);
            if (characterPosInit != characterTarget.GetComponent<CharacterStateController>().positionOfCharacter)
            {
                path[0].GetComponent<GridStat>().isDestinationForEntity = false;
                path[etapeMvmtIA].GetComponent<GridStat>().hasEntityOnIt = false;
                path[etapeMvmtIA].GetComponent<GridStat>().nbrEntityOnIt -= 1;
                return Exit(new Follow());
            }
            return Exit(new Attack());   
        }

        else if (IsIAarrivedEtape(etapeMvmtIA, path))
        {
            oneTime = true;
            if (Vector3.Distance(characterTarget.transform.position, transform.position) < rangeToAttackPlayer && path[etapeMvmtIA].GetComponent<GridStat>().nbrEntityOnIt==1)
            {
                Debug.Log("aaaaaaa");
                path[0].GetComponent<GridStat>().isDestinationForEntity = false;
                transform.gameObject.GetComponent<Animator>().SetBool("isWalking", false);
                return Exit(new Attack());
            }
            path[etapeMvmtIA].GetComponent<GridStat>().hasEntityOnIt = false;
            path[etapeMvmtIA].GetComponent<GridStat>().nbrEntityOnIt -= 1;
            etapeMvmtIA--;
            positionOfCharacter = path[etapeMvmtIA].GetComponent<GridStat>().posInGridArray;
            return this;
        }
        else
        {
            path[etapeMvmtIA].GetComponent<GridStat>().hasEntityOnIt = true;
            if (oneTime)
            {
                path[etapeMvmtIA].GetComponent<GridStat>().nbrEntityOnIt += 1;
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
