using System.Collections.Generic;
using UnityEngine;

public class UnitSelections : MonoBehaviour
{
    public List<GameObject> unitList = new List<GameObject>();
    public List<GameObject> unitsSelected = new List<GameObject>();


    private static UnitSelections _instance;
    public static UnitSelections Instance { get { return _instance; } }

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void ClickSelect(GameObject unitToAdd)
    {
        DeselectAll();
        unitsSelected.Add(unitToAdd);
        unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void ShiftClickSelect(GameObject unitToAdd)
    {
        if(!unitsSelected.Contains(unitToAdd)) 
        {
            unitsSelected.Add(unitToAdd);
            unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            unitToAdd.transform.GetChild(0).gameObject.SetActive(false);
            unitsSelected.Remove(unitToAdd);
        }
    }

    public void DragSelect(GameObject unitToAdd)
    {
        if(!unitsSelected.Contains(unitToAdd))
        {
            unitsSelected.Add(unitToAdd);
            unitToAdd.transform.GetChild (0).gameObject.SetActive(true);
        }
    }

    public void DeselectAll()
    {
        foreach(GameObject unit in unitsSelected) 
        {
            unit.transform.GetChild(0).gameObject.SetActive(false);
            unit.GetComponent<CharacterStateController>().selected = false;
        }
        unitsSelected.Clear();
    }
}
