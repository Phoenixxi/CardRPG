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

    public bool isForked = false; // False if there is only one choice for node, true if there is a choice to be made
    public bool forkUnlocked = false;// initially, both are false, and a popup should appear. Once true no popups
    public bool isClosed;// true for the node not chosen, cannot move to it.
    public NodeController thisNode; // Assign in Inspector for the "A" choice node, or leave null if not forked
    public NodeController otherNode; // Assign in Inspector for the "B" choice node, or leave null if not forked



    // Start is called before the first frame update
    void Start()
    {
        UpdateLightState();

    }

    // Update is called once per frame
void Update()
{
    float distance = Vector3.Distance(transform.position, player.transform.position);
    UpdateLightState();

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

    if (isForked && forkUnlocked)
    {
        // Only check for forked choices if thisNode and otherNode are not null
        if (thisNode != null && otherNode != null)
        {
            if (!isClosed && Input.GetKeyDown(KeyCode.Alpha1)) // 1 for Node A
            {
                SelectThisNode();
            }
            else if (!isClosed && Input.GetKeyDown(KeyCode.Alpha2)) // 2 for Node B
            {
                SelectOtherNode();
            }
        }
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
        // If the node has a fork, show options to choose from
        if (isForked && forkUnlocked)
        {
            // Only proceed if thisNode and otherNode are not null and unlocked
            if (thisNode != null && !thisNode.isClosed)
            {
                SelectThisNode();
            }
            else if (otherNode != null && !otherNode.isClosed)
            {
                SelectOtherNode();
            }
        }
        else if (nodeUnlocked && !isForked)
        {
            // No fork, regular movement
            FindObjectOfType<PlayerController>().MoveToNode(transform.position);
            FindObjectOfType<CameraController>().MoveCameraToNode(this);
        }

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

        // Function to select the "Node A" and lock the "Node B" path
    private void SelectThisNode()
    {
        if (thisNode != null)
        {
            thisNode.nodeUnlocked = true;
            thisNode.isClosed = false; // This node is now open
            otherNode.isClosed = true;

            // Optional: move player and camera to Node A if needed
            FindObjectOfType<PlayerController>().MoveToNode(thisNode.transform.position);
            FindObjectOfType<CameraController>().MoveCameraToNode(thisNode);
            Debug.Log("You selected Node A.");
        }
    }

    // Function to select the "Node B" and lock the "Node A" path
    private void SelectOtherNode()
    {
        if (otherNode != null)
        {
            otherNode.nodeUnlocked = true;
            otherNode.isClosed = false; // This node is now open
            thisNode.isClosed = true;

            // Optional: move player and camera to Node B if needed
            FindObjectOfType<PlayerController>().MoveToNode(otherNode.transform.position);
            FindObjectOfType<CameraController>().MoveCameraToNode(otherNode);
            Debug.Log("You selected Node B.");
        }
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
