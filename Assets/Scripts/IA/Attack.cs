using UnityEngine;

class Attack : CharacterState
{
    private GameObject playerTarget;
    private int posPlayer;
    public override CharacterState Enter(Transform characterT, int posCharacter, float s, float t, float r, GameObject[] g)
    {
        base.Enter(characterT, posCharacter, s, t, r, g);
        playerTarget = GetPlayerTransform();
        if (playerTarget == null)
        {
            return Exit(new Idle());
        }
        posPlayer = playerTarget.GetComponent<CharacterStateController>().positionOfCharacter;
        transform.LookAt(new Vector3(gridArray[posPlayer].transform.position.x,
                                         transform.position.y,
                                         gridArray[posPlayer].transform.position.z));
        transform.gameObject.GetComponent<Animator>().SetBool("isAttacking", true);
        return this;
    }
    
    public override CharacterState UpdateState()
    {
        base.UpdateState();
        gridArray[positionOfCharacter].GetComponent<GridStat>().hasEntityOnIt = true;
        if (posPlayer != playerTarget.GetComponent<CharacterStateController>().positionOfCharacter)
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
