using System.Collections.Generic;
using UnityEngine;

public class GridStat : MonoBehaviour
{
    public Dictionary<string, int> v;

    public int posInGridArray;

    public GameObject[] voisins;

    private float radiusToFindVoisins;

    public bool hasEntityOnIt = false;
    public bool isDestinationForEntity = false;
    public int nbrEntityOnIt = 0;

    private void Awake()
    {
        GetPlatformVoisins();
    }

    private void GetPlatformVoisins()
    {
        v = new Dictionary<string, int>();
        voisins = new GameObject[6];
        radiusToFindVoisins = 0.866f;

        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position,
                               radiusToFindVoisins,
                               transform.forward,
                               radiusToFindVoisins);
        int i = 0;
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.position != transform.position && hit.collider.gameObject.layer == 6 && hit.transform.gameObject.tag != "Hole")
            {
                voisins[i] = hit.transform.gameObject;
                i++;
            }
        }
    }
}
