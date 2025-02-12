using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{public static MapManager Instance { get; private set; }
    
    public List<NodeController> nodes = new List<NodeController>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        PopulateNodes();
    }

    private void PopulateNodes()
    {
        nodes.Clear(); // Clears existing list before adding
        nodes.AddRange(FindObjectsOfType<NodeController>());
    }
}
