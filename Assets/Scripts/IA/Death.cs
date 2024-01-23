using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

class Death : CharacterState
{
    private bool waiting = false;
    private float time;

    private Collider collider;
    private Animator animator;
    private CharacterStateController characterStateController;
    public override CharacterState Enter(Transform characterT, int posCharacter, float s, float t, float r, float ra, GridStat[] g)
    {
        base.Enter(characterT, posCharacter, s, t, r, ra, g);
        animator = transform.GetComponent<Animator>();
        collider = transform.GetComponent<Collider>();
        characterStateController = transform.GetComponent<CharacterStateController>();
        animator.SetBool("isDead", true);
        collider.enabled = false;
        gridArray[posCharacter].hasEntityOnIt = false;

        if (characterStateController.possessFlag)
        {
            if (transform.gameObject.CompareTag("Player"))
            {
                characterStateController.Lose();
            }

            GameObject.Find("Game Manager").GetComponent<FlagSpawner>().SpawnFlag(transform.gameObject);
            characterStateController.possessFlag = false;
            characterStateController.FlagImg.gameObject.SetActive(false);
        }
        transform.gameObject.tag = "Untagged";
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
