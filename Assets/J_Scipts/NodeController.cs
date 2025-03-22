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
    public bool playerIsOverNode = false; 
    public GameObject player; // Assign in Inspector
    public float detectionRadius = 0.005f;
    public static NodeController activeNode = null; // Track the closest node
    public bool nodeUnlocked = false; // Assign in inspector
    public Light nodeLight; // Assign light in the Inspector


    // Start is called before the first frame update
    void Start()
    {
        UpdateLightState();

    }

    // Update is called once per frame
void Update()
{
    float distance = Vector3.Distance(transform.position, player.transform.position);

    if (nodeUnlocked && distance <= detectionRadius)
    {
        playerIsOverNode = true;
        activeNode = this; // Set this node as the currently active one
    }
    else if (activeNode == this) // If the player moves away, reset activeNode
    {
        playerIsOverNode = false;
        activeNode = null;
    }

    // Only allow Enter key to work on the closest active node
    if (activeNode == this && nodeUnlocked && Input.GetKeyDown(KeyCode.Return)) 
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

     // Function to unlock the node
    public void UnlockNode()
    {
        nodeUnlocked = true;
        UpdateLightState();
    }

        public void LockNode()
    {
        nodeUnlocked = false;
        UpdateLightState();
    }


    // Turn light on/off based on node state
    private void UpdateLightState()
    {
        if (nodeLight != null)
        {
            nodeLight.enabled = nodeUnlocked; // Light is enabled when node is unlocked
        }
    }
}
