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
        if (player == null)
        {
            // Debug.LogWarning("Player reference is missing. Ensure it is assigned.");
            return; // Prevent further execution if player is null
        }

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
            // Handle node selection if forked
            if (thisNode != null && !thisNode.isClosed)
            {
                SelectNode(thisNode, otherNode);
            }
            else if (otherNode != null && !otherNode.isClosed)
            {
                SelectNode(otherNode, thisNode);
            }
            // Handle if there is a scene to load
            else if(!string.IsNullOrEmpty(sceneToLoad))
            {
                LoadNextScene();
            }
        }
    }

        // Function to select a node and lock the other node path
    private void SelectNode(NodeController selectedNode, NodeController unselectedNode)
    {
        if (selectedNode != null)
        {
            selectedNode.nodeUnlocked = true;
            selectedNode.isClosed = false; // This node is now open
            if (unselectedNode != null)
            {
                unselectedNode.isClosed = true; // Lock the other node
                unselectedNode.nodeUnlocked = false; // Ensure it's not selectable
            }

            // Optional: Move player and camera to the selected node
            FindObjectOfType<PlayerController>().MoveToNode(selectedNode.transform.position);
            FindObjectOfType<CameraController>().MoveCameraToNode(selectedNode);
        }
    }


    private void LoadNextScene()
    {
        MapManager.Instance.SaveGameData(); // Save node states before switching scenes
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        // else
        // {
        //     Debug.LogWarning("No scene assigned for this node.");
        // }
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
        // Prevent actions if the node is closed
        if (isClosed)
        {
            return; // Do nothing if the node is closed
        }

        // If the node has a fork, show options to choose from
        if (isForked && forkUnlocked)
        {
            // Only proceed if thisNode and otherNode are not null and unlocked
            // if (thisNode != null && !thisNode.isClosed)
            // {
            //     SelectNode(thisNode, otherNode);
            // }
            // if (otherNode != null && !otherNode.isClosed)
            // {
            //     SelectNode(otherNode, thisNode);
            // }
            FindObjectOfType<PlayerController>().MoveToNode(transform.position);
            FindObjectOfType<CameraController>().MoveCameraToNode(this);
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

    // Turn light on/off based on node state
    public void UpdateLightState()
    {
        if (nodeLight != null)
    {
            if (isClosed)
                nodeLight.enabled = false;
            else // Light is enabled when the node is unlocked and not closed
                nodeLight.enabled = nodeUnlocked;
        }
    }
}
