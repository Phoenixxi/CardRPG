using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CardNamespace;

public class VictoryLossManager : MonoBehaviour
{
    public bool winLossStatus = false;
 
 void Start()
 {
    winLossStatus = false;
    GameManager gameManager = FindObjectOfType<GameManager>();
    if(gameManager != null)
    {
        gameManager.SetVictoryLossManager(this);
    }

 }
    public void Victory()
    {
        //FIX THIS DIRECTORY
        winLossStatus = true;
        // for victory screen
    }


    public void Loss()
    {
        //FIX THIS DIRECTORY
        winLossStatus = false;
        // for loss screen
    }
}
