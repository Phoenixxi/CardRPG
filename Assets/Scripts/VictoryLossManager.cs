using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryLossManager : MonoBehaviour
{
    public bool winLossStatus = false;
    void Start()
    {
        winLossStatus = false;
    }
    public void Victory()
    {
        //FIX THIS DIRECTORY
        winLossStatus = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
    }


    public void Loss()
    {
        winLossStatus = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
    }
}
