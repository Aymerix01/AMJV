using UnityEngine;

public class TrapScript : MonoBehaviour
{
    [SerializeField] private float attackDmg;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<CharacterStateController>().pv -= attackDmg;
            Destroy(transform.gameObject);
        }
    }
}
