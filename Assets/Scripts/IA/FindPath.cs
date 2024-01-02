using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class FindPath
{

    public static GameObject[] GetPathIA(Transform characterTransform, int startOfTheTravel, int endOfTheTravel, GameObject[] gridArray)
    {
        string nameGameObject = characterTransform.gameObject.name;
        List<GameObject> path = new List<GameObject>();

        if (gridArray[endOfTheTravel] != null)
        {
            gridArray[endOfTheTravel].GetComponent<GridStat>().isDestinationForEntity = true;
            SetDistance(nameGameObject, startOfTheTravel, gridArray);
            SetPath(nameGameObject, startOfTheTravel, endOfTheTravel, gridArray, path);
        }
        return path.ToArray();
    }

    private static void SetDistance(string nameGameObject, int startOfTheTravel, GameObject[] gridArray)
    {
        gridArray = InitialSetup(nameGameObject, startOfTheTravel, gridArray);
        for (int step = 1; step < gridArray.Length; step++)
        {
            foreach (GameObject platforme in gridArray)
            {
                if (platforme && platforme.GetComponent<GridStat>().v[nameGameObject] == step - 1)
                {
                    TestDirection(nameGameObject, step, platforme);
                }
            }
        }
    }

    private static GameObject[] InitialSetup(string nameGameObject, int startOfTheTravel, GameObject[] gridArray)
    {
        foreach (GameObject platform in gridArray)
        {
            if (platform != null && platform.name != "Hole")
            {
                platform.GetComponent<GridStat>().v[nameGameObject] = -1;
            }
        }
        gridArray[startOfTheTravel].GetComponent<GridStat>().v[nameGameObject] = 0;
        return gridArray;
    }

    private static void TestDirection(string nameGameObject, int step, GameObject platforme)
    {
        foreach (GameObject platformeVoisine in platforme.GetComponent<GridStat>().voisins)
        {
            if (platformeVoisine && platformeVoisine.GetComponent<GridStat>().v[nameGameObject] == -1)
            {
                SetVisited(nameGameObject, step, platformeVoisine);
            }
        }
    }

    private static void SetVisited(string nameGameObject, int step, GameObject platforme)
    {
        if (platforme != null && platforme.name != "Hole")
        {
            platforme.GetComponent<GridStat>().v[nameGameObject] = step;
        }
    }

    private static List<GameObject> SetPath(string nameGameObject, int startOfTheTravel, int endOfTheTravel, GameObject[] gridArray, List<GameObject> path)
    {
        int step;
        int fin = endOfTheTravel;
        List<GameObject> list = new List<GameObject>();
        path.Clear();
        if (gridArray[endOfTheTravel] && gridArray[endOfTheTravel].GetComponent<GridStat>().v[nameGameObject] > 0)
        {
            path.Add(gridArray[fin]);
            step = gridArray[fin].GetComponent<GridStat>().v[nameGameObject] - 1;
        }
        else
        {
            //Debug.Log(nameGameObject);
            //Debug.Log(endOfTheTravel);
            //Debug.Log("Can't reach the desired location");
            path.Add(gridArray[startOfTheTravel]);
            return path;
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
            GameObject tempObj = FindClosest(gridArray[endOfTheTravel].transform, gridArray, list);
            path.Add(tempObj);
            fin = tempObj.GetComponent<GridStat>().posInGridArray;
            list.Clear();
        }
        return path;
    }

    private static GameObject FindClosest(Transform targetLocation, GameObject[] gridArray, List<GameObject> list)
    {
        float currentDistance = gridArray.Length;
        int indexNumber = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (Vector3.Distance(targetLocation.position, list[i].transform.position) < currentDistance)
            {
                currentDistance = Vector3.Distance(targetLocation.position, list[i].transform.position);
                indexNumber = i;
            }
        }
        return list[indexNumber];
    }
}
