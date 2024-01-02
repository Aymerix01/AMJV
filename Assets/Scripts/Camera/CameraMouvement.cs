using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouvement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3f;
    [SerializeField]
    private float rotationSpeed = 10f;
    [SerializeField]
    private float zoomSpeed = 5f;

    private Vector3 zoomDir = new Vector3(0, -Mathf.Sin(Mathf.Deg2Rad*20), Mathf.Cos(Mathf.Deg2Rad * 20));

    private void Start()
    {
        //Time.timeScale = 10f;
    }

    void Update()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalMovement, 0f, verticalMovement).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.Self);

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime, Space.World);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(zoomDir * zoomInput * zoomSpeed, Space.Self);
    }
}
