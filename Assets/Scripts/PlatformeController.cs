using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformeController : MonoBehaviour
{
    [SerializeField]
    private float hoverHeight = 0.1f;

    private float originalY;

    [SerializeField]
    private bool hasEntityOnIt = false;
    
    private void Start()
    {
        originalY = 0f;
    }

    public void EntityOnIt(bool entityOnIt)
    {
        hasEntityOnIt = entityOnIt;
        transform.position = new Vector3(transform.position.x, originalY, transform.position.z);
    }
    public bool HasEntityOnIt() {  return hasEntityOnIt; }

    private void OnMouseEnter()
    {
        if (!hasEntityOnIt)
            transform.position = new Vector3(transform.position.x, originalY + hoverHeight, transform.position.z);
    }

    private void OnMouseExit()
    {
        transform.position = new Vector3(transform.position.x, originalY, transform.position.z);
    }
}
