using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
     public float nodeZoomLevel = 60f; // Default zoom level for this node
    public Vector3 cameraOffset; // Custom position offset per node
    public Vector3 cameraRotation; // Custom rotation per node
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        //Debug.Log("Node Clicked: " + gameObject.name);
        // Move Player to Node
        FindObjectOfType<PlayerController>().MoveToNode(transform.position);
        // Tell the camera to move & rotate
        FindObjectOfType<CameraController>().MoveCameraToNode(this);

    }
}
