using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform player;  // Assign Player in Inspector
    public float smoothSpeed = 5.0f;  // Adjust for smoother camera movement
    public Camera mainCamera;  // Reference to the main camera

    Vector3 offset;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private float targetFOV;
    public float zoomSmoothSpeed = 2.0f; // Speed of zoom transition


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;  // Get the main camera if not already assigned

        // Set the initial camera position to match your desired position
        // transform.position = fixedCameraPosition;

        offset = player.position - transform.position;
        targetPosition = transform.position;
        targetRotation = transform.rotation;

        targetFOV = mainCamera.fieldOfView; // Set initial FOV

    }

    // Update is called once per frame
    // void Update()
    // {
    // }

    //private void FixedUpdate(){
    // Calculate the target position by adding the offset to the player's position
    //Vector3 targetPosition = player.position - offset;

    // Smoothly move the camera to the target position
    //}

        void LateUpdate()
    {
        // Smoothly move the camera to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // Smoothly rotate the camera
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);

        // Smoothly adjust zoom level (FOV)
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, smoothSpeed * Time.deltaTime);

    }

    public void MoveCameraToNode(NodeController node)
    {
        targetPosition = node.transform.position + node.cameraOffset; // Move to the node's custom position
        targetRotation = Quaternion.Euler(node.cameraRotation); // Rotate camera to the node's custom rotation
        targetFOV = node.nodeZoomLevel; // Change FOV based on node settings
    }


}
