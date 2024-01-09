using System.Collections;
using UnityEngine;

public class PaysanAttackSpe : MonoBehaviour
{
    [SerializeField] private GameObject trap;
    [SerializeField] private float timeSpawnTrap;
    private bool waiting;
    private CharacterStateController characterStateController;

    private void Start()
    {
        waiting = false;
        characterStateController = GetComponent<CharacterStateController>();
    }

    private void Update()
    {
        if(!waiting && Input.GetKeyDown(KeyCode.R) && characterStateController.selected)
        {
            StartCoroutine(Wait());
            GameObject trapInstance = Instantiate(trap);
            trapInstance.transform.position = transform.position;
        }
    }

    private IEnumerator Wait()
    {
        waiting = true;
        yield return new WaitForSeconds(timeSpawnTrap);
        waiting = false;
    }
}
