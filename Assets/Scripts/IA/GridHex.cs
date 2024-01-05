using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class GridHex : CharacterState
{
    public static GameObject[] GetGrid(Transform characterTransform)
    {
        string nameGameObject = characterTransform.gameObject.name;
        GameObject gridPrefab = GameObject.Find("Grid");
        int nbrPlatforme = gridPrefab.transform.GetChild(0).childCount;
        GameObject[] gridArray = new GameObject[nbrPlatforme];

        gridArray = InitGrid(nameGameObject, nbrPlatforme, gridPrefab, gridArray);
        return gridArray;
    }

    private static GameObject[] InitGrid(string nameGameObject, int nbrPlatforme, GameObject gridPrefab, GameObject[] gridArray)
    {
        for (int i = 0; i < nbrPlatforme; i++)
        {
            GameObject platform = gridPrefab.transform.GetChild(0).GetChild(i).gameObject;
            if (platform.tag != "Hole")
            {
                gridArray[i] = platform;
                gridArray[i].GetComponent<GridStat>().posInGridArray = i;
                if (!gridArray[i].GetComponent<GridStat>().v.ContainsKey(nameGameObject))
                {
                    gridArray[i].GetComponent<GridStat>().v.Add(nameGameObject, -1);
                } 
                else
                {
                    gridArray[i].GetComponent<GridStat>().v[nameGameObject] = -1;
                }
            }
        }
        return gridArray;
    }

}
