using System.Collections.Generic;
using UnityEngine;

public class UnitSelections : MonoBehaviour
{
    public List<GameObject> unitList = new List<GameObject>();
    public List<GameObject> unitsSelected = new List<GameObject>();
    [SerializeField] private Canvas loseScreen;
    [SerializeField] private Canvas BoxGrid;


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
    private void Update()
    {
        if (unitList.Count <= 0) 
        {
            loseScreen.GetComponent<Transform>().GetChild(1).gameObject.SetActive(true);
            BoxGrid.gameObject.SetActive(false);
            loseScreen.GetComponent<UIManager>().victoire = true;
            Time.timeScale = 0f;
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
            if (unit != null)
            {
                unit.transform.GetChild(0).gameObject.SetActive(false);
                unit.GetComponent<CharacterStateController>().selected = false;
            }
        }
        unitsSelected.Clear();
    }
}
