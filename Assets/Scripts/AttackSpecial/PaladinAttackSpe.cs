using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinAttackSpe : MonoBehaviour
{
    [SerializeField] private float timeEndBoost;
    [SerializeField] private float timeSpeAttack;
    [SerializeField] private float radiusSpeAttack;
    [SerializeField] private float boostArmor;

    private List<GameObject> playerBoosted;

    private bool waiting;
    private bool boostActivated;
    private bool oneTime;
    private CharacterStateController characterStateController;

    private void Start()
    {
        waiting = false;
        boostActivated = false;
        oneTime = false;
        playerBoosted = new List<GameObject>();
        characterStateController = GetComponent<CharacterStateController>();
    }

    private void Update()
    {
        if (!waiting && Input.GetKeyDown(KeyCode.R) && characterStateController.selected)
        {
            StartCoroutine(WaitAttack());
            oneTime = true;
            RaycastHit[] hits;
            hits = Physics.SphereCastAll(transform.position,
                                   radiusSpeAttack,
                                   transform.forward,
                                   radiusSpeAttack);
            foreach (RaycastHit hit in hits)
            {
                GameObject obj = hit.collider.gameObject;
                if (obj.CompareTag("Player"))
                {
                    playerBoosted.Add(obj);
                    obj.GetComponent<CharacterStateController>().armor += boostArmor;
                }
            }
            StartCoroutine(WaitEndBoost());
        }
        if(!boostActivated && waiting && oneTime)
        {
            oneTime = false;
            foreach (GameObject obj in playerBoosted)
            {
                if (obj != null)
                {
                    obj.GetComponent<CharacterStateController>().armor -= boostArmor;
                }
            }
            playerBoosted.Clear();
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
