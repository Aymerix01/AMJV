using UnityEngine;

class Attack : CharacterState
{
    public override CharacterState Enter(Transform characterT, int posCharacter, float s, float t, float r, GameObject[] g)
    {
        base.Enter(characterT, posCharacter, s, t, r, g);
        //transform.gameObject.GetComponent<Animator>().SetBool("isAttacking", true);
        return this;
    }

    public override CharacterState UpdateState()
    {
        base.UpdateState();
        return this;
    }
}
