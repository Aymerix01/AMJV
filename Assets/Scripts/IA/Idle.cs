using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Idle : CharacterState
{
    private bool waiting = false;
    private float time;
    private List<GameObject> unitsSelected;
    private void Wait()
    {
        waiting = true;
        if (timeWaiting < time)
        {
            waiting = false;
        }
        time += Time.deltaTime;
    }

    public override CharacterState Enter(Transform characterT, int posCharacter, float s, float t, float r, float ra, GameObject[] g)
    {

        base.Enter(characterT, posCharacter, s, t, r, ra, g);
        unitsSelected = GameObject.FindWithTag("Game Manager").GetComponentInChildren<UnitSelections>().unitsSelected;
        return this;
    }

    public override CharacterState UpdateState()
    {
        base.UpdateState();
        Wait();
        gridArray[positionOfCharacter].GetComponent<GridStat>().hasEntityOnIt = true;

        if (transform.gameObject.GetComponent<CharacterStateController>().pv <= 0)
        {
            return Exit(new Death());
        }
        if ( transform.gameObject.layer == 7 && unitsSelected.Contains(transform.gameObject))

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
        if (SeeEnemy())
        {
            GameObject[] enemiesSeen = GetEnemiesGameObject();
            foreach (GameObject enemy in enemiesSeen)
            {
                int childCount = enemy.transform.childCount;
                for (int i = 1; i < childCount; i++)
                {
                    enemy.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
        return this;
    }
}
