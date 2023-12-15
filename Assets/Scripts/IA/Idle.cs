using System.Collections;
using UnityEngine;

class Idle : CharacterState
{
    private bool waiting = false;
    private float time;
    private void Wait()
    {
        waiting = true;
        if (timeWaiting < time)
        {
            waiting = false;
        }
        time += Time.deltaTime;
    }

    public override CharacterState Enter(Transform characterT, int posCharacter, float s, float t, float r, GameObject[] g)
    {
        base.Enter(characterT, posCharacter, s, t, r, g);
        return this;
    }

    public override CharacterState UpdateState()
    {
        base.UpdateState();
        Wait();
        if (!waiting)
        {
            return Exit(new Walk());
        }
        if (SeePlayer())
        {
            return Exit(new Follow());
        }
        return this;
    }
}
