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
    public int thisWorld = 0; //first node map is world 0, second is world 1, and final world is world 2
    public bool isBossNode = false; //Set True if the Node ID is the final boss
    private string nextWorldScene = null;
    private bool worldCleared = false;
    public NodeController thisNode; // Assign in Inspector for the "A" choice node, or make it this node if no fork
    public NodeController otherNode; // Assign in Inspector for the "B" choice node, or leave null if not forked
    public List<NodeController> nextNode; //If there is only one node ahead, it will have one. if it is forked, there will be two.
    public int ID = 0; //0 is default. 1 is node1

    public List<NodeController> prevNodes;     // Only assign in the node following a fordked node 2 elements: [ node3A, node3B ]

    public List<ParticleSystem> pathParticle; //If there is only one node ahead, it will have one. if it is forked, there will be two.
    private bool initialized = false;
    public GameObject characterDisplayCanvas;
    public GameObject characterAFolder;
    public GameObject characterBFolder;
    private bool isCharacterSelectionActive = false;
    private bool worldChanged = false;


    // Start is called before the first frame update
    void Start()
    {
        UpdateLightState();
        characterDisplayCanvas.SetActive(true);

        // thisWorld = 0;
        // isBossNode = false;
    }

    void OnDisable()
    {
        
    }

    public NodeController sendCurrentNode()
    {
        return activeNode;
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

            Debug.Log("jared status " + GameManager.Instance.VictoryLossManager.winLossStatus);
            // Debug.Log("Received Win status");
            if (GameManager.Instance.VictoryLossManager.winLossStatus && activeNode.ID == 1)// First Fight world 1
            {
                thisNode.nextNode[0].nodeUnlocked = true;
                // Debug.Log("Unlocking next node");
                winLossStatusReceived = true;
            }
            else if (GameManager.Instance.VictoryLossManager.winLossStatus && (thisWorld == 0) && (activeNode.ID == 6))//Final fight world 1
            {
                // thisNode.nextNode[0].nodeUnlocked = true;
                isBossNode = true;
                winLossStatusReceived = true;
                worldCleared = true;
                thisWorld = 1;
                nextWorldScene = "MainMenu";
                LoadNextWorld();
                GameObject mapRoot = GameObject.Find("World1");
                if (mapRoot != null)
                {
                    mapRoot.SetActive(false);
                }
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
        if (activeNode == this && nodeUnlocked)
        {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (thisNode != null && !thisNode.isClosed)
                    {
                        SelectNode(thisNode, otherNode);
                    }

                    if (!string.IsNullOrEmpty(sceneToLoad))
                    {
                        LoadScene();
                    }

                    if (this.isDialogue)
                    {
                        PlayDialogue();
                    }

                    isCharacterSelectionActive = false;
                    HideCharacterDisplay();
                }

            // If character display is up, allow Enter or Left-Click to interact
            if (isCharacterSelectionActive)
            {
                if (Input.GetMouseButtonDown(0)) // Left-click cancels character display
                {
                    HideCharacterDisplay();
                    isCharacterSelectionActive = false;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Return)) // Normal behavior if display not up
            {
                ShowCharacterDisplay();
                isCharacterSelectionActive = true;
            }
        }
    }
    private void ShowCharacterDisplay()
    {
        if (characterDisplayCanvas == null) return;

        Transform folderToShow = null;

        if (ID == 3)
            folderToShow = characterDisplayCanvas.transform.Find("KingFolder");
        else if (ID == 4)
            folderToShow = characterDisplayCanvas.transform.Find("BellaFolder");

        foreach (Transform child in characterDisplayCanvas.transform)
        {
            child.gameObject.SetActive(false);
        }

        if (folderToShow != null)
        {
            folderToShow.gameObject.SetActive(true);
        }
    }

    private void HideCharacterDisplay()
    {
        if (characterDisplayCanvas == null) return;

        foreach (Transform child in characterDisplayCanvas.transform)
        {
            child.gameObject.SetActive(false);
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

            //Forked Node world 1
            if (this.thisWorld == 0 && ((this.ID == 3) || (this.ID == 4)) && (this.nextNode[0].ID == 5))
            {
                // Debug.LogWarning("We should unlock node 4 ID 5");
                thisNode.nextNode[0].nodeUnlocked = true;
            }
            //Forked Node world 2
            else if (this.thisWorld == 1 && ((this.ID == 2) || (this.ID == 3)) && (this.nextNode[0].ID == 4))
            {
                // Debug.LogWarning("We should unlock node 4 ID 5");
                thisNode.nextNode[0].nodeUnlocked = true;
            }

            UpdateLightState();
            // UpdateVFXState();
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
    }

    private void LoadNextWorld()
    {
        MapManager.Instance.SaveGameData();

        if (!string.IsNullOrEmpty(nextWorldScene) && isBossNode)
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
        // isCharacterSelectionActive = false;
        if (isForked && nodeUnlocked)
        {
            ShowCharacterDisplay();
            isCharacterSelectionActive = true;

            FindObjectOfType<PlayerController>().MoveToNode(transform.position);
            FindObjectOfType<CameraController>().MoveCameraToNode(this);

            if (isDialogue)
            {
                PlayDialogue();
            }

            return; // Stop here to avoid double logic for forked display
        }

        if (nodeUnlocked)
        {
            FindObjectOfType<PlayerController>().MoveToNode(transform.position);
            FindObjectOfType<CameraController>().MoveCameraToNode(this);
        }

        foreach (Transform child in characterDisplayCanvas.transform)
        {
            child.gameObject.SetActive(false); // Hide all
        }
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
        // keep vfx in sync with the light/node state:
        UpdateVFXState();

    }

    private void UpdateVFXState()
    {
        if (pathParticle == null || pathParticle.Count == 0) return;
        if (isClosed)
            this.pathParticle[0].Stop();

        //First Update, turn everything off
        if (initialized == false)
        {
            // Debug.LogWarning("First Time");

            if (!nodeUnlocked && this.pathParticle[0].isPlaying)
            {
                this.pathParticle[0].Stop();
                // Debug.LogWarning(this.ID + " Am Stopping");
            }

            if (!nodeUnlocked && this.pathParticle.Count == 2)
            {
                this.pathParticle[1].Stop();
                // Debug.LogWarning(this.ID + " Am Forked and stopping");
            }
            initialized = true;
        }
        // nodes after fork update
        if (prevNodes != null && prevNodes.Count == pathParticle.Count)
        {
            for (int i = 0; i < prevNodes.Count; i++)
            {
                if (pathParticle[i] == null || prevNodes[i] == null)
                    continue;

                // only play the one branch that was unlocked
                if (prevNodes[i].nodeUnlocked && !prevNodes[i].isClosed)
                    pathParticle[i].Play();
                else
                    pathParticle[i].Stop();
            }
            return;
        }
        //Every other nodes update
        if (nodeUnlocked && !this.pathParticle[0].isPlaying)
            this.pathParticle[0].Play();
    }



    private void PlayDialogue()
    {
        if (this.isDialogue && dialogueScript != null && !dialogueScript.IsDialogueActive())
        {
            // Start dialogue when over a dialogue node, only if dialogue is not active
            dialogueScript.gameObject.SetActive(true);
            dialogueScript.StartDialogue();
            //World 1 Dialogue
            if (thisWorld == 0)
            {
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
            }
            //World 2 Dialogue
            else if (this.thisWorld == 1)
            {
                if (this.ID == 1 && this.nextNode.Count >= 1 && (this.nextNode[0].ID == 2 && this.nextNode[1].ID == 3))
                {
                    this.nextNode[0].nodeUnlocked = true;
                    this.nextNode[1].nodeUnlocked = true;
                }
            }
            MapManager.Instance.SaveGameData(); // Save node states before switching scenes
        }
    }
}
