using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Grid grid;

    private Vector3Int? positionInGrid;
    private Vector3Int? destinationInGrid;
    
    void Start()
    {
        positionInGrid = null;
        destinationInGrid = null;
        Time.timeScale = 1f;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && positionInGrid != null)
        {
            destinationInGrid = getTilePosition();
        }

        if (Input.GetMouseButtonDown(0) && positionInGrid == null) 
        {
            positionInGrid = getTilePosition();
        }

        if (destinationInGrid != null && positionInGrid != null) 
        {
            StartCoroutine(Move());
            positionInGrid = null;
            destinationInGrid = null;
        }

        if (Input.GetMouseButtonDown(1))
        {
            positionInGrid = null;
            destinationInGrid = null;
        }
    }

    Vector3Int? getTilePosition()
    {
        Vector3Int? tilePosition = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            int layer = hit.collider.gameObject.layer;
            if (layer == 6) //layer 6 == platform
            {
                GameObject tile = hit.collider.gameObject;
                tilePosition = grid.LocalToCell(tile.transform.position);
            }
        }
        return tilePosition;
    }

    IEnumerator Move()
    {
        yield return new WaitForSeconds(1);
    }
}
