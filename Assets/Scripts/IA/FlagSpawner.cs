using UnityEngine;

public class FlagSpawner : MonoBehaviour
{
    [SerializeField] GameObject flag;

    public void SpawnFlag(GameObject deadCharacter)
    {
        Instantiate(flag, deadCharacter.transform.position + new Vector3(0, deadCharacter.transform.localScale.y/2, 0), deadCharacter.transform.rotation);
    }
}
