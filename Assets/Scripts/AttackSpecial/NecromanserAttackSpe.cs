using System.Collections;
using UnityEngine;

public class NecromanserAttackSpe : MonoBehaviour
{
    [SerializeField] private GameObject skeletons;
    [SerializeField] private float timeSpawnSkeletons = 20;
    private bool waiting;

    public void Update()
    {
        if (!waiting && gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(Wait() );
            SpawnSkeletons(gameObject.GetComponent<CharacterStateController>().positionOfCharacter, transform.position);
        }
    }
    public void SpawnSkeletons(int posI, Vector3 posV)
    {
        GameObject refSkeletonSpawn = Instantiate(skeletons, posV, transform.rotation);
        for (int i = 0;i<refSkeletonSpawn.transform.childCount;i++)
        {
            refSkeletonSpawn.transform.GetChild(i).gameObject.SetActive(false);
        }
        refSkeletonSpawn.GetComponent<CharacterStateController>().positionOfCharacter = posI;
    }

    IEnumerator Wait()
    {
        waiting = true;
        yield return new WaitForSeconds(timeSpawnSkeletons);
        waiting = false;
    }
}
