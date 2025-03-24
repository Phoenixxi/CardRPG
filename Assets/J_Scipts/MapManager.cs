using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{public static MapManager Instance { get; private set; }
    
    public List<NodeController> nodes = new List<NodeController>();
    public PlayerController player;
    public CameraController cameraController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Prevents MapManager from being destroyed when changing scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        PopulateNodes();
    }

    private void Start()
    {
        //SaveGameData(); // Save 
        //PlayerPrefs.DeleteAll();  // Reset for Testing

        player = FindObjectOfType<PlayerController>();
        cameraController = FindObjectOfType<CameraController>();
        LoadGameData();
        // Make sure we move the camera to the active node
        if (NodeController.activeNode != null)
        {
            // Debug.Log("Moving camera to node: " + NodeController.activeNode.name);
            cameraController.MoveCameraToNode(NodeController.activeNode);
        }
        // else
        // {
            // Debug.LogWarning("activeNode is null");
            // NodeController.activeNode = nodes[0];
        // }
    }


    private void PopulateNodes()
    {
        nodes.Clear(); // Clears existing list before adding
        nodes.AddRange(FindObjectsOfType<NodeController>());
        nodes.Sort((node1, node2) => string.Compare(node1.name, node2.name));
    }

    public void SaveGameData()
    {
        // Save the active node's name (or an ID if using integer identifiers)
        if (NodeController.activeNode != null)
        {
            PlayerPrefs.SetString("ActiveNodeName", NodeController.activeNode.name);
        }

        // Save Player Position
        PlayerPrefs.SetFloat("Player_Pos_X", player.transform.position.x);
        PlayerPrefs.SetFloat("Player_Pos_Y", player.transform.position.y);
        PlayerPrefs.SetFloat("Player_Pos_Z", player.transform.position.z);

        // Save Camera Position & Rotation
        PlayerPrefs.SetFloat("Camera_Pos_X", cameraController.transform.position.x);
        PlayerPrefs.SetFloat("Camera_Pos_Y", cameraController.transform.position.y);
        PlayerPrefs.SetFloat("Camera_Pos_Z", cameraController.transform.position.z);

        PlayerPrefs.SetFloat("Camera_Rot_X", cameraController.transform.rotation.eulerAngles.x);
        PlayerPrefs.SetFloat("Camera_Rot_Y", cameraController.transform.rotation.eulerAngles.y);
        PlayerPrefs.SetFloat("Camera_Rot_Z", cameraController.transform.rotation.eulerAngles.z);

        // Save Camera FOV
        PlayerPrefs.SetFloat("Camera_FOV", cameraController.mainCamera.fieldOfView);

        // Save Node Data
        foreach (var node in nodes)
        {
            PlayerPrefs.SetInt($"Node_{node.name}_Unlocked", node.nodeUnlocked ? 1 : 0);
            PlayerPrefs.SetInt($"Node_{node.name}_Closed", node.isClosed ? 1 : 0);
        }

        PlayerPrefs.Save();
    }

    public void LoadGameData()
    {

        // Load the active node's name from PlayerPrefs
        string activeNodeName = PlayerPrefs.GetString("ActiveNodeName", string.Empty);

        // Find the node by its name
        NodeController activeNode = nodes.Find(node => node.name == activeNodeName);
                // Load Node Data - Reset all nodes to locked by default
        foreach (var node in nodes)
        {
            bool isUnlocked = PlayerPrefs.GetInt($"Node_{node.name}_Unlocked", 0) == 1;
            node.nodeUnlocked = isUnlocked; // Set to true if saved, else false
            node.isClosed = PlayerPrefs.GetInt($"Node_{node.name}_Closed", 0) == 1;
            node.UpdateLightState(); // Update visuals
        }
        bool firstNodeUnlocked = PlayerPrefs.GetInt($"Node_{nodes[0].name}_Unlocked", -1) == 1;
        if (firstNodeUnlocked == false)
        {
            // Unlock the first node by default if no saved data is found
            nodes[0].nodeUnlocked = true;
        }
        else
        {

            foreach (var node in nodes)
            {
                node.nodeUnlocked = PlayerPrefs.GetInt($"Node_{node.name}_Unlocked", 0) == 1;
                node.isClosed = PlayerPrefs.GetInt($"Node_{node.name}_Closed", 0) == 1;
                node.UpdateLightState(); // Update visuals
            }
        }

        // If a matching node is found, set it as the active node
        if (activeNode != null)
        {
            NodeController.activeNode = activeNode;
        }
        else
        {
            // Debug.LogWarning("Active node not found or no active node saved.");
            NodeController.activeNode = nodes[0];
        }

        // Load Player Position
        Vector3 playerPosition = new Vector3(
            PlayerPrefs.GetFloat("Player_Pos_X", player.transform.position.x),
            PlayerPrefs.GetFloat("Player_Pos_Y", player.transform.position.y),
            PlayerPrefs.GetFloat("Player_Pos_Z", player.transform.position.z)
        );
        player.transform.position = playerPosition;
        
        // Call to move the camera to the loaded active node
        if (NodeController.activeNode != null)
        {
            cameraController.MoveCameraToNode(NodeController.activeNode);
        }


        // Load Camera Position & Rotation
        Vector3 cameraPosition = new Vector3(
            PlayerPrefs.GetFloat("Camera_Pos_X", cameraController.transform.position.x),
            PlayerPrefs.GetFloat("Camera_Pos_Y", cameraController.transform.position.y),
            PlayerPrefs.GetFloat("Camera_Pos_Z", cameraController.transform.position.z)
        );
        cameraController.transform.position = cameraPosition;

        Vector3 cameraRotation = new Vector3(
            PlayerPrefs.GetFloat("Camera_Rot_X", cameraController.transform.rotation.eulerAngles.x),
            PlayerPrefs.GetFloat("Camera_Rot_Y", cameraController.transform.rotation.eulerAngles.y),
            PlayerPrefs.GetFloat("Camera_Rot_Z", cameraController.transform.rotation.eulerAngles.z)
        );
        cameraController.transform.rotation = Quaternion.Euler(cameraRotation);

        // Load Camera FOV
        cameraController.mainCamera.fieldOfView = PlayerPrefs.GetFloat("Camera_FOV", cameraController.mainCamera.fieldOfView);

    }

}
