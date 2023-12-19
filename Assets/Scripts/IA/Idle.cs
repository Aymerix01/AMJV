using System.Collections;
using UnityEngine;

class Idle : CharacterState
{
    private bool waiting = false;
    private float time;
    private UnitSelections unitSelections;
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
        unitSelections = GameObject.FindWithTag("Game Manager").GetComponentInChildren<UnitSelections>();
        return this;
    }

    public override CharacterState UpdateState()
    {
        base.UpdateState();
        Wait();
        gridArray[positionOfCharacter].GetComponent<GridStat>().hasEntityOnIt = true;
        if ( transform.gameObject.layer == 7 && unitSelections.unitsSelected.Contains(transform.gameObject))
        {
            return Exit(new Selected());
        }
        if (!waiting && transform.gameObject.layer != 7)
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
