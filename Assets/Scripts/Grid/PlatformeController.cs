using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformeController : MonoBehaviour
{
    [SerializeField] private float hoverHeight = 0.1f;

    private float originalYofPlatform;

    /*[HideInInspector]*/ public bool hasEntityOnIt = false;
    [HideInInspector] public bool isDestinationForEntity = false;
    
    private void Start()
    {
        originalYofPlatform = 0f;
    }
/*
    private void OnMouseEnter()
    {
        if (!hasEntityOnIt)
            transform.position = new Vector3(transform.position.x, originalYofPlatform + hoverHeight, transform.position.z);
    }

    private void OnMouseExit()
    {
        transform.position = new Vector3(transform.position.x, originalYofPlatform, transform.position.z);
    }
*/
}
