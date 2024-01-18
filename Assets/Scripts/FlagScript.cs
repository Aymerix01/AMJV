using UnityEngine;

public class FlagScript : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<CharacterStateController>().possessFlag = true;
            other.gameObject.GetComponent<CharacterStateController>().FlagImg.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
