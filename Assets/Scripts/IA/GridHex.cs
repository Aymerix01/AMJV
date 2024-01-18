using UnityEngine;

class GridHex : CharacterState
{
    public static GridStat[] GetGrid(Transform characterTransform)
    {
        string nameGameObject = characterTransform.gameObject.name;
        GameObject gridPrefab = GameObject.Find("Grid");
        int nbrPlatforme = gridPrefab.transform.GetChild(0).childCount;
        GridStat[] gridArray = new GridStat[nbrPlatforme];

        gridArray = InitGrid(nameGameObject, nbrPlatforme, gridPrefab, gridArray);
        return gridArray;
    }

    private static GridStat[] InitGrid(string nameGameObject, int nbrPlatforme, GameObject gridPrefab, GridStat[] gridArray)
    {
        for (int i = 0; i < nbrPlatforme; i++)
        {
            GameObject platformGO = gridPrefab.transform.GetChild(0).GetChild(i).gameObject;
            if (platformGO.tag != "Hole")
            {
                GridStat platform = gridPrefab.transform.GetChild(0).GetChild(i).gameObject.GetComponent<GridStat>();
                gridArray[i] = platform;
                gridArray[i].posInGridArray = i;
                if (!gridArray[i].v.ContainsKey(nameGameObject))
                {
                    gridArray[i].v.Add(nameGameObject, -1);
                } 
                else
                {
                    gridArray[i].v[nameGameObject] = -1;
                }
            }
        }
        return gridArray;
    }

}
