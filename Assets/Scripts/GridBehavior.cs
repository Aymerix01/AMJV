using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GridBehavior : MonoBehaviour
{
    private int scale = 1;
    private GameObject gridPrefab;

    private int start = 58;
    private int end = 2;
    private List<GameObject> path = new List<GameObject>();

    private string nameGameObject;
    [SerializeField] private int pos;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float timeWaiting = 10f;

    private bool waiting = false;
    private int nbrPlatforme;
    private int etapeMvmtIA;

    private Animator anim;

    [HideInInspector] public GameObject[] gridArray;

    private void Start()
    {
        nameGameObject = transform.gameObject.name+pos;
        start = 58;
        anim = GetComponent<Animator>();
        gridPrefab = GameObject.Find("Grid");
        nbrPlatforme = gridPrefab.transform.GetChild(0).childCount;
        gridArray = new GameObject[nbrPlatforme];
        GetGrid();
        SetIApos();
        GetPathIA();
        etapeMvmtIA = path.Count - 1;
    }

    private void Update()
    {
        if (!waiting && isIAarrivedEtape(0))
        {
            anim.SetBool("isWalking", false);
            StartCoroutine(Wait());
            GetPathIA();
            etapeMvmtIA = path.Count - 1;
        } 
        else if (!waiting && isIAarrivedEtape(etapeMvmtIA))
        {
            etapeMvmtIA--;
        } 
        else if (!waiting)
        {
            anim.SetBool("isWalking", true);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(path[etapeMvmtIA].transform.position.x, 0.34f + path[etapeMvmtIA].transform.position.y, path[etapeMvmtIA].transform.position.z), speed * Time.deltaTime);
            transform.LookAt(new Vector3(path[etapeMvmtIA].transform.position.x, transform.position.y, path[etapeMvmtIA].transform.position.z));
        }
    }

    private void GetGrid()
    {
        for (int i = 0; i < nbrPlatforme; i++) 
        {
            GameObject platform = gridPrefab.transform.GetChild(0).GetChild(i).gameObject;
            if (platform.name != "Hole") 
            {
                gridArray[i] = platform;
                gridArray[i].GetComponent<GridStat>().posInGridArray = i;
                gridArray[i].GetComponent<GridStat>().v.Add(nameGameObject, -1);
            }
        }
    }

    private void SetDistance()
    {
        InitialSetup();
        for(int step=1; step<nbrPlatforme; step++)
        {
            foreach(GameObject platforme in gridArray)
            {
                if (platforme && platforme.GetComponent<GridStat>().v[nameGameObject] == step-1)
                {
                    TestDirection(platforme, step);
                }
            }
        }
    }

    private void InitialSetup()
    {
        foreach (GameObject platform in gridArray)
        {
            if (platform != null && platform.name != "Hole")
            {
                platform.GetComponent<GridStat>().v[nameGameObject] = -1;
            }
        }
        gridArray[start].GetComponent<GridStat>().v[nameGameObject] = 0;
    }

    private void TestDirection(GameObject platforme, int step)
    {
        foreach(GameObject platformeVoisine in platforme.GetComponent<GridStat>().voisins)
        {
            if (platformeVoisine && platformeVoisine.GetComponent<GridStat>().v[nameGameObject] == -1)
            {
                SetVisited(platformeVoisine, step);
            } 
        }
    }

    private void SetVisited(GameObject platforme, int step)
    {
        if (platforme != null && platforme.name != "Hole")
        {
            platforme.GetComponent<GridStat>().v[nameGameObject] = step;
        }
    }

    private void SetPath()
    {
        int step;
        int fin = end;
        List<GameObject> list = new List<GameObject>();
        path.Clear();
        if (gridArray[end] && gridArray[end].GetComponent<GridStat>().v[nameGameObject] > 0)
        {
            path.Add(gridArray[fin]);
            step = gridArray[fin].GetComponent<GridStat>().v[nameGameObject] - 1;
        }
        else
        {
            Debug.Log("Can't reach the desired location");
            return;
        }

        for (int i = step; i > -1; i--)
        {
            foreach (GameObject platformeVoisine in gridArray[fin].GetComponent<GridStat>().voisins)
            {
                if (platformeVoisine && platformeVoisine.GetComponent<GridStat>().v[nameGameObject] == i)
                {
                    list.Add(platformeVoisine);
                }
            }
            GameObject tempObj = FindClosest(gridArray[end].transform, list);
            path.Add(tempObj);
            fin = tempObj.GetComponent<GridStat>().posInGridArray;
            list.Clear();
        }
    }

    private GameObject FindClosest(Transform targetLocation, List<GameObject> list)
    {
        float currentDistance = scale * nbrPlatforme;
        int indexNumber = 0;
        for(int i=0;i<list.Count; i++)
        {
            if(Vector3.Distance(targetLocation.position, list[i].transform.position) < currentDistance)
            {
                currentDistance = Vector3.Distance(targetLocation.position, list[i].transform.position);
                indexNumber = i;
            }
        }
        return list[indexNumber];
    }

    IEnumerator Wait()
    {
        waiting = true;
        yield return new WaitForSeconds(timeWaiting);
        waiting = false;
    }

    private void SetIApos()
    {
        transform.position = new Vector3(gridArray[pos].transform.position.x, 0.34f + gridArray[pos].transform.position.y, gridArray[pos].transform.position.z);
    }

    private void GetPathIA()
    {
        int dest = Random.Range(0, gridArray.Length - 1);
        while (gridArray[dest] == null)
        {
            dest = Random.Range(0, gridArray.Length - 1);
        }

        if (gridArray[dest] != null)
        {
            start = pos;
            end = dest;

            SetDistance();
            SetPath();
        }
    }

    private bool isIAarrivedEtape(int etape)
    {
        if(transform.position == new Vector3(path[etape].transform.position.x, 0.34f + path[etape].transform.position.y, path[etape].transform.position.z))
        {
            return true;
        } else
        {
            return false;
        }
    }
}
