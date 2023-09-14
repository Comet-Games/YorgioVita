using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float zoomSpeed = 5f;
    public float minZoom = 2f;
    public float maxZoom = 10f;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Camera Movement
        float horizontalInput = Input.GetAxis("Left Stick Horizontal");
        float verticalInput = Input.GetAxis("Left Stick Vertical");
        float horizontalInputPC = Input.GetAxis("Horizontal");
        float verticalInputPC = Input.GetAxis("Vertical");

        // Use joystick inputs for PS Vita, and keyboard inputs for testing on computer
        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f);
        Vector3 moveDirectionPC = new Vector3(horizontalInputPC, verticalInputPC, 0f);

        // Normalize the direction vector to ensure consistent speed in all directions
        moveDirection.Normalize();
        moveDirectionPC.Normalize();

        // Move the camera
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        transform.Translate(moveDirectionPC * moveSpeed * Time.deltaTime);


        // Camera Zoom using right joystick (up and down)
        float zoomInput = Input.GetAxis("Right Stick Vertical");

        // Use mouse scroll wheel for zoom on the computer
        if (Mathf.Abs(zoomInput) > 0.1f)
        {
            // Adjust the zoom based on the right joystick input (up or down)
            mainCamera.orthographicSize -= -zoomInput * zoomSpeed;

            // Clamp the zoom to stay within the minZoom and maxZoom range
            mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, minZoom, maxZoom);
        }
    }
}
