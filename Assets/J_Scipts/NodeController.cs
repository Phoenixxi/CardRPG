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
    public bool isClosed;// true for the node not chosen, cannot move to it.

    public bool isDialogue = false; //False in general, true if dialogue;
    public Dialogue dialogueScript;  // Reference to the Dialogue script (assign in Inspector)
    private bool winLossStatusReceived = false;
    public int thisWorld = 0; //first node map is world 0, second is world 1, and final world is world 3
    public bool isBossNode = false; //Set True if the Node ID is the final boss
    private string nextWorldScene = null;
    private bool worldCleared = false;

    public NodeController thisNode; // Assign in Inspector for the "A" choice node, or make it this node if no fork
    public NodeController otherNode; // Assign in Inspector for the "B" choice node, or leave null if not forked
    public List<NodeController> nextNode; //If there is only one node ahead, it will have one. if it is forked, there will be two.
    public int ID = 0; //0 is default. 1 is node1

    // Start is called before the first frame update
    void Start()
    {
        UpdateLightState();
        // thisWorld = 0;
        // isBossNode = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            // Debug.LogWarning("Player reference is missing.");
            return; // Prevent further execution if player is null
        }

        if (GameManager.Instance != null && GameManager.Instance.VictoryLossManager != null && !winLossStatusReceived)
        {
            if (GameManager.Instance.VictoryLossManager.winLossStatus && thisNode.ID == 1)// First Fight world 1
            {
                thisNode.nextNode[0].nodeUnlocked = true;
                winLossStatusReceived = true;
            }
            else if (GameManager.Instance.VictoryLossManager.winLossStatus && (thisWorld == 0) && (thisNode.ID == 6))//Final fight world 1
            {
                // thisNode.nextNode[0].nodeUnlocked = true;
                isBossNode = true;
                winLossStatusReceived = true;
                worldCleared = true;
                thisWorld = 1;
                // nextWorldScene = "MainMenu";
                // LoadScene();
            }

        }

        float distance = Vector3.Distance(transform.position, player.transform.position);
        UpdateLightState();

        // Update Player Moved for this node
        if (nodeUnlocked && distance <= detectionRadius)
        {
            playerIsOverNode = true;
            activeNode = this;
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
                {SelectNode(thisNode, otherNode);}

            // Handle loading scenes
            if(!string.IsNullOrEmpty(sceneToLoad))
                {LoadScene();}
            // Handle 
            if (this.isDialogue == true)
                {PlayDialogue();}
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

            if (((this.ID == 3) || (this.ID == 4)) && (this.nextNode[0].ID == 5))
            {
                // Debug.LogWarning("We should unlock node 4 ID 5");
                thisNode.nextNode[0].nodeUnlocked = true;
            }
            UpdateLightState();
            MapManager.Instance.SaveGameData(); // Save node states before switching scenes
        }
    }


    private void LoadScene()
    {
        MapManager.Instance.SaveGameData(); // Save node states before switching scenes
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        if(nextWorldScene!= null)
        {
            SceneManager.LoadScene(nextWorldScene);
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
        // Prevent actions if the node is closed
        if (isClosed)
        {
            return; // Do nothing if the node is closed
        }
        if (isForked && nodeUnlocked && isDialogue)
        {
            // FindObjectOfType<PlayerController>().MoveToNode(transform.position);
            // FindObjectOfType<CameraController>().MoveCameraToNode(this);
            // Make a new method to be invoked, not here, but for SelectNode and onEnter
            PlayDialogue();
        }
        else if (nodeUnlocked)
        {
            FindObjectOfType<PlayerController>().MoveToNode(transform.position);
            FindObjectOfType<CameraController>().MoveCameraToNode(this);
        }

        // If the node has a fork, show options to choose from
        // else if (nodeUnlocked && !isForked)
        // {
        //     // No fork, regular movement
        //     FindObjectOfType<PlayerController>().MoveToNode(transform.position);
        //     FindObjectOfType<CameraController>().MoveCameraToNode(this);
        // }
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

    private void PlayDialogue()
    {
        if (this.isDialogue && dialogueScript != null && !dialogueScript.IsDialogueActive())
        {
            // Start dialogue when over a dialogue node, only if dialogue is not active
            dialogueScript.gameObject.SetActive(true);
            dialogueScript.StartDialogue();

            // After a dialogue node is entered -> unlock node condition
            if (this.ID == 2 && this.nextNode.Count >= 2 && this.nextNode[0].ID == 3 && this.nextNode[1].ID == 4)
            {
                this.nextNode[0].nodeUnlocked = true;
                this.nextNode[1].nodeUnlocked = true;
            }
            // Other dialogue node unlock condition
            else if (this.ID == 5 && this.nextNode.Count >= 1 && this.nextNode[0].ID == 6)
            {
                this.nextNode[0].nodeUnlocked = true;
            }
            MapManager.Instance.SaveGameData(); // Save node states before switching scenes
        }
    }
}
