using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardNamespace;

public class VLManager : MonoBehaviour
{
    public GameObject victoryScreen;
    public GameObject lossScreen;
    public bool status = false;

    void Start()
    {
        if(status)
        {
            victoryScreen.SetActive(true);
            lossScreen.SetActive(false);
        }
        else
        {
            victoryScreen.SetActive(false);
            lossScreen.SetActive(true);
        }

    }
}
