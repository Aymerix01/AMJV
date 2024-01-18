using UnityEngine;

public class Finish : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<CharacterStateController>().possessFlag)
        {
            Debug.Log("Victoire");
        }
    }
}
