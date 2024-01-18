using UnityEngine;

public class UnitClick : MonoBehaviour
{
    private Camera myCam;

    public LayerMask clickable;

    void Start()
    {
        myCam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        { 
            RaycastHit hit;
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
            {
                if(Input.GetKey(KeyCode.LeftShift))
                {
                    UnitSelections.Instance.ShiftClickSelect(hit.collider.gameObject);
                }
                else
                {
                    UnitSelections.Instance.ClickSelect(hit.collider.gameObject);
                } 
            }
            else
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    UnitSelections.Instance.DeselectAll();
                }
            }
        }
    }
}
