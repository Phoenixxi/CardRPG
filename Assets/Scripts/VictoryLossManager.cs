using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CardNamespace;

public class VictoryLossManager : MonoBehaviour
{
    public bool winLossStatus = false;
    public VLManager vLManager;
    void Start()
    {
        winLossStatus = false;
    }
    public void Victory()
    {
        //FIX THIS DIRECTORY
        winLossStatus = true;
        // for victory screen
        vLManager.status = true;
        SceneManager.LoadScene("VictoryLossScene");
    }


    public void Loss()
    {
        //FIX THIS DIRECTORY
        winLossStatus = false;
        // for loss screen
        vLManager.status = false;
        SceneManager.LoadScene("VictoryLossScene");
    }
}
