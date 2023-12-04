using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject entity;

    private Ray ray;

    private void Start()
    {
        entity.transform.localScale = new Vector3(0.28f, 0.28f, 0.28f);
    }

    private void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetButtonDown("Fire1"))
        {
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.tag == "Platforme")
                {
                    PlatformeController platformeController = hit.collider.GetComponent<PlatformeController>();
                    if (!platformeController.HasEntityOnIt())
                    {
                        /*
                        Vector3 spawnPos = new Vector3(hit.transform.position.x, 0.24f + hit.transform.position.y, hit.transform.position.z);
                        Instantiate(entity, spawnPos, new Quaternion(0, 180, 0, 0));
                        platformeController.EntityOnIt(true);*/
                    } 
                }
            }
        }
    }
}
