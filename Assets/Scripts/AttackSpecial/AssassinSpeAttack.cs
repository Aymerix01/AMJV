using System.Collections;
using UnityEngine;

public class AssassinSpeAttack : MonoBehaviour
{
    [SerializeField] private float timeSpeAttack;
    [SerializeField] private float timeEndBoost;
    [SerializeField] private float boost;

    private bool waiting;
    private bool boostActivated;
    private bool oneTime;
    private CharacterStateController characterStateController;

    private void Start()
    {
        waiting = false;
        boostActivated = false;
        oneTime = false;
        characterStateController = GetComponent<CharacterStateController>();
    }

    private void Update()
    {
        if (!waiting && Input.GetKeyDown(KeyCode.R) && characterStateController.selected)
        {
            oneTime = true;
            StartCoroutine(WaitAttack());
            characterStateController.attackDmg += boost;
            StartCoroutine(WaitEndBoost());
        }
        if(!boostActivated && waiting && oneTime)
        {
            oneTime = false;
            characterStateController.attackDmg -= boost;
        }
    }

    private IEnumerator WaitEndBoost()
    {
        boostActivated = true;
        yield return new WaitForSeconds(timeEndBoost);
        boostActivated = false;
    }
    private IEnumerator WaitAttack()
    {
        waiting = true;
        yield return new WaitForSeconds(timeSpeAttack);
        waiting = false;

    }
}
