using System.Collections.Generic;
using UnityEngine;

class Attack : CharacterState
{
    private GameObject characterTarget;
    private int posPlayer;
    private List<GameObject> unitsSelected;
    public override CharacterState Enter(Transform characterT, int posCharacter, float s, float t, float r, float ra, GameObject[] g)
    {
        base.Enter(characterT, posCharacter, s, t, r, ra, g);
        unitsSelected = GameObject.FindWithTag("Game Manager").GetComponentInChildren<UnitSelections>().unitsSelected;
        if (transform.gameObject.tag == "Enemy")
        {
            characterTarget = GetPlayerTransform();
            if (characterTarget == null)
            {

                return Exit(new Idle());
            }
            posPlayer = characterTarget.GetComponent<CharacterStateController>().positionOfCharacter;
            transform.LookAt(new Vector3(gridArray[posPlayer].transform.position.x,
                                             transform.position.y,
                                             gridArray[posPlayer].transform.position.z));
            transform.gameObject.GetComponent<Animator>().SetBool("isAttacking", true);
            return this;
        }
        else if (transform.gameObject.tag == "Player")
        {
            characterTarget = transform.gameObject.GetComponent<CharacterStateController>().opponentToAttack;
            if (characterTarget == null)
            {
                return Exit(new Idle());
            }
            posPlayer = characterTarget.GetComponent<CharacterStateController>().positionOfCharacter;
            transform.LookAt(new Vector3(gridArray[posPlayer].transform.position.x,
                                             transform.position.y,
                                             gridArray[posPlayer].transform.position.z));
            transform.gameObject.GetComponent<Animator>().SetBool("isAttacking", true);
            return this;
        }
        return this;
        
    }
    
    public override CharacterState UpdateState()
    {
        base.UpdateState();
        gridArray[positionOfCharacter].GetComponent<GridStat>().hasEntityOnIt = true;
        if (transform.gameObject.GetComponent<CharacterStateController>().pv <= 0)
        {
            return Exit(new Death());
        }
        if (Input.GetMouseButtonDown(1) && unitsSelected.Contains(transform.gameObject))
        {
            transform.gameObject.GetComponent<Animator>().SetBool("isAttacking", false);
            return Exit(new Walk());
        }
        if (characterTarget.GetComponent<CharacterStateController>().pv <= 0)
        {
            transform.gameObject.GetComponent<Animator>().SetBool("isAttacking", false);
            return Exit(new Idle());
        }
        if (Vector3.Distance(characterTarget.transform.position,transform.position) > rangeToAttackPlayer)
        {
            transform.gameObject.GetComponent<Animator>().SetBool("isAttacking", false);
            return Exit(new Follow());
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            return Exit(new Death());
        }
        return this;
    }
}
