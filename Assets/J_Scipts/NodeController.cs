using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NodeController : MonoBehaviour
{
    public float nodeZoomLevel = 60f; // Default zoom level for this node
    public Vector3 cameraOffset; // Custom position offset per node
    public Vector3 cameraRotation; // Custom rotation per node
    public string sceneToLoad; // Scene name to load when entering the node
    private bool playerIsOverNode = true; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsOverNode && Input.GetKeyDown(KeyCode.Return)) 
        {
            LoadNextScene();
        }

    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("No scene assigned for this node.");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure your player has the "Player" tag
        {
            playerIsOverNode = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsOverNode = false;
        }
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
