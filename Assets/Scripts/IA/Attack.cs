using UnityEngine;

class Attack : CharacterState
{
    public override CharacterState Enter(Transform characterT, int posCharacter, float s, float t, float r, GameObject[] g)
    {
        base.Enter(characterT, posCharacter, s, t, r, g);
        transform.LookAt(new Vector3(gridArray[0].transform.position.x,
                                         transform.position.y,
                                         gridArray[0].transform.position.z));
        transform.gameObject.GetComponent<Animator>().SetBool("isAttacking", true);
        return this;
    }

    public override CharacterState UpdateState()
    {
        base.UpdateState();
        if (Input.GetKeyUp(KeyCode.Space))
        {
            return Exit(new Death());
        }
        return this;
    }
}
