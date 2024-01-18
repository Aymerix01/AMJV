using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

class Death : CharacterState
{
    private bool waiting = false;
    private float time;
    public override CharacterState Enter(Transform characterT, int posCharacter, float s, float t, float r, float ra, GameObject[] g)
    {
        base.Enter(characterT, posCharacter, s, t, r, ra, g);
        transform.gameObject.GetComponent<Animator>().SetBool("isDead", true);
        transform.gameObject.tag = "Untagged";
        transform.gameObject.GetComponent<Collider>().enabled = false;
        gridArray[posCharacter].gameObject.GetComponent<GridStat>().hasEntityOnIt = false;
        if (characterT.gameObject.GetComponent<CharacterStateController>().possessFlag)
        {
            GameObject.Find("Game Manager").GetComponent<FlagSpawner>().SpawnFlag(characterT.gameObject);
            characterT.gameObject.GetComponent<CharacterStateController>().possessFlag = false;
            characterT.gameObject.GetComponent<CharacterStateController>().FlagImg.gameObject.SetActive(false);
        }
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
