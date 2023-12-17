using System.Collections;
using System.Collections.Generic;
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
    }

    
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //do something
        }

        if (Input.GetMouseButtonDown(0) && positionInGrid != null)
        {
            destinationInGrid = getTilePosition();
            Debug.Log("Destination : " + destinationInGrid);
        }

        if (Input.GetMouseButtonDown(0) && positionInGrid == null) 
        {
            positionInGrid = getTilePosition();
            Debug.Log("Position : " + positionInGrid);
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
            //layer 6 == platform
            if (layer == 6)
            {
                //Obtention de la position sur la tilemap
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
