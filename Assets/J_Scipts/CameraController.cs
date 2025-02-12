using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform player;  // Assign Player in Inspector
    public Vector3 fixedCameraPosition = new Vector3(-3.04f, 1.1f, -1.88f);  // Set this to match your desired zoom level
    public float smoothSpeed = 5.0f;  // Adjust for smoother camera movement
    public Camera mainCamera;  // Reference to the main camera

    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        //mainCamera = Camera.main;  // Get the main camera if not already assigned

        // Set the initial camera position to match your desired position
        //transform.position = fixedCameraPosition;

        offset = player.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void FixedUpdate(){
    // Calculate the target position by adding the offset to the player's position
    //Vector3 targetPosition = player.position - offset;

    // Smoothly move the camera to the target position
    //}

        void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(player.position.x - offset.x, transform.position.y, transform.position.z);
        transform.position = targetPosition;
    }

}
