using UnityEngine;

public class CameraMouvement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3f;
    [SerializeField]
    private float rotationSpeed = 10f;
    [SerializeField]
    private float zoomSpeed = 5f;
    [SerializeField]
    private float minZoom = 3f;
    [SerializeField]
    private float maxZoom = 10f;

    private Vector3 zoomDir = new Vector3(0, -Mathf.Sin(Mathf.Deg2Rad*20), Mathf.Cos(Mathf.Deg2Rad * 20));

    void Update()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");

        Vector3 moveDirection = new Vector3(horizontalMovement, 0f, verticalMovement);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.Self);

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime, Space.World);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }
        if (transform.position.y <= minZoom)
        {
            zoomInput = zoomInput >= 0 ? 0 : zoomInput;
        }
        else if (transform.position.y >= maxZoom)
        {
            zoomInput = zoomInput >= 0 ? zoomInput : 0;
        }
        transform.Translate(zoomDir * zoomInput * zoomSpeed, Space.Self);
    }
}
