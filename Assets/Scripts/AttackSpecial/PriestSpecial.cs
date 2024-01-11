using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestSpecial : MonoBehaviour
{
    [SerializeField] private float timeSpeAttack;
    [SerializeField] private float radiusSpeAttack;
    [SerializeField] private float heal;

    private List<GameObject> playerHealed;

    private bool waiting;
    private CharacterStateController characterStateController;

    private void Start()
    {
        waiting = false;
        playerHealed = new List<GameObject>();
        characterStateController = GetComponent<CharacterStateController>();
    }

    private void Update()
    {
        if (!waiting && Input.GetKeyDown(KeyCode.R) && characterStateController.selected)
        {
            StartCoroutine(WaitAttack());
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
                    playerHealed.Add(obj);
                    obj.GetComponent<CharacterStateController>().health += heal;
                }
            }
        }
    }
    private IEnumerator WaitAttack()
    {
        waiting = true;
        yield return new WaitForSeconds(timeSpeAttack);
        waiting = false;

    }
}
