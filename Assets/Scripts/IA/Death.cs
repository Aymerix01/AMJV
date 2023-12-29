using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

class Death : CharacterState
{
    private bool waiting = false;
    private float time;
    public override CharacterState Enter(Transform characterT, int posCharacter, float s, float t, float r, GameObject[] g)
    {
        base.Enter(characterT, posCharacter, s, t, r, g);
        transform.gameObject.GetComponent<Animator>().SetBool("isDead", true);
        transform.gameObject.tag = "Untagged";
        gridArray[posCharacter].gameObject.GetComponent<GridStat>().hasEntityOnIt = false;
        return this;
    }

    public override CharacterState UpdateState()
    {
        base.UpdateState();
        Wait();
        if (!waiting)
        {
            GameObject.Destroy(transform.gameObject);
        }
        return this;
    }

    private void Wait()
    {
        waiting = true;
        if (10 < time)
        {
            waiting = false;
        }
        time += Time.deltaTime;
    }
}
