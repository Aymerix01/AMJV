using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrouillardGuerre : MonoBehaviour
{
    private Renderer rObj;
    private Renderer[] rObjChild;
    private float radiusToshow = 5;
    private bool oneTime;
    private void Start()
    {
        oneTime = true;
        rObj = gameObject.GetComponent<Renderer>();
        rObjChild = gameObject.GetComponentsInChildren<Renderer>();
        rObj.enabled = false;

        foreach (Renderer g in rObjChild)
        {
            g.enabled = false;
        }
    }

    private void Update()
    {
        if (oneTime)
        {
            RaycastHit[] hits;
            hits = Physics.SphereCastAll(transform.position,
                                   radiusToshow,
                                   transform.forward,
                                   radiusToshow);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    rObj.enabled = true;
                    foreach (Renderer g in rObjChild)
                    {
                        g.enabled = true;
                    }
                    oneTime = false;
                }
            }
        }
    }
}
