using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardNamespace;

public class VLManager : MonoBehaviour
{
    public GameObject victoryEnemyScreen;
    public GameObject victorySviurScreen;
    public GameObject victoryEstellaScreen;
    public GameObject lossScreen;
    public bool status = false;
    private int worldID;
    private bool isBossBattle;

    void Start()
    {
        NodeController activeNode = MapManager.Instance.nodes[0].sendCurrentNode();
        worldID = activeNode.thisWorld;
        isBossBattle = activeNode.isBossNode;

        if(status)
        {
            victoryEnemyScreen.SetActive(true);
            lossScreen.SetActive(false);
        }
        else
        {
            victoryEnemyScreen.SetActive(false);
            lossScreen.SetActive(true);
        }

    }

    
}
