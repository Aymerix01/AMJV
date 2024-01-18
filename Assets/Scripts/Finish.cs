using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private GameObject victoryCanvas;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<CharacterStateController>().possessFlag)
        {
            victoryCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
