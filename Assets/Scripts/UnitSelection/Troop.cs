using UnityEngine;

public class Troop : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UnitSelections.Instance.unitList.Add(this.gameObject);
    }

    // Update is called once per frame
    void OnDestroy()
    {
        UnitSelections.Instance.unitList.Remove(this.gameObject);
    }
}
