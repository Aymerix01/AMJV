using System.Collections.Generic;
using UnityEngine;

class FindPath
{
    public static GridStat[] GetPathIA(Transform characterTransform, int startOfTheTravel, int endOfTheTravel, GridStat[] gridArray)
    {
        string nameGameObject = characterTransform.gameObject.name;
        List<GridStat> path = new List<GridStat>();
        if (gridArray[endOfTheTravel] != null)
        {
            gridArray[endOfTheTravel].isDestinationForEntity = true;
            SetDistance(nameGameObject, startOfTheTravel, gridArray);
            SetPath(nameGameObject, startOfTheTravel, endOfTheTravel, gridArray, path);
        }
        return path.ToArray();
    }

    private static void SetDistance(string nameGameObject, int startOfTheTravel, GridStat[] gridArray)
    {
        gridArray = InitialSetup(nameGameObject, startOfTheTravel, gridArray);
        for (int step = 1; step < gridArray.Length; step++)
        {
            foreach (GridStat platforme in gridArray)
            {
                if (platforme != null && platforme.v[nameGameObject] == step - 1)
                {
                    TestDirection(nameGameObject, step, platforme);
                }
            }
        }
    }

    private static GridStat[] InitialSetup(string nameGameObject, int startOfTheTravel, GridStat[] gridArray)
    {
        foreach (GridStat platform in gridArray)
        {
            if (platform != null && platform.gameObject.name != "Hole")
            {
                platform.v[nameGameObject] = -1;
            }
        }
        gridArray[startOfTheTravel].v[nameGameObject] = 0;
        return gridArray;
    }

    private static void TestDirection(string nameGameObject, int step, GridStat platforme)
    {
        foreach (GridStat platformeVoisine in platforme.voisins)
        {
            if (platformeVoisine != null && platformeVoisine.v[nameGameObject] == -1)
            {
                SetVisited(nameGameObject, step, platformeVoisine);
            }
        }
    }

    private static void SetVisited(string nameGameObject, int step, GridStat platforme)
    {
        if (platforme != null && platforme.name != "Hole")
        {
            platforme.GetComponent<GridStat>().v[nameGameObject] = step;
        }
    }

    private static List<GridStat> SetPath(string nameGameObject, int startOfTheTravel, int endOfTheTravel, GridStat[] gridArray, List<GridStat> path)
    {
        int step;
        int fin = endOfTheTravel;
        List<GridStat> list = new List<GridStat>();
        path.Clear();
        if (gridArray[endOfTheTravel] && gridArray[endOfTheTravel].v[nameGameObject] > 0)
        {
            path.Add(gridArray[fin]);
            step = gridArray[fin].v[nameGameObject] - 1;
        }
        else
        {
            path.Add(gridArray[startOfTheTravel]);
            return path;
        }
        for (int i = step; i > -1; i--)
        {
            foreach (GridStat platformeVoisine in gridArray[fin].voisins)
            {
                if (platformeVoisine && platformeVoisine.v[nameGameObject] == i)
                {
                    list.Add(platformeVoisine);
                }
            }
            GridStat tempObj = FindClosest(gridArray[endOfTheTravel].transform, gridArray, list);
            path.Add(tempObj);
            fin = tempObj.posInGridArray;
            list.Clear();
        }
        return path;
    }

    private static GridStat FindClosest(Transform targetLocation, GridStat[] gridArray, List<GridStat> list)
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
