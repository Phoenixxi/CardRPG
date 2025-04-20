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

    void Start()
    {

        victoryEnemyScreen.SetActive(false);
        victorySviurScreen.SetActive(false);
        victoryEstellaScreen.SetActive(false);
        lossScreen.SetActive(false);

    }

    public void VictoryEnemy()
    {
        victoryEnemyScreen.SetActive(true);
    }

    public void VictorySviur()
    {
        victorySviurScreen.SetActive(true);
    }

    public void VictoryEstella()
    {
        victoryEstellaScreen.SetActive(true);
    }

    public void Loss()
    {
        lossScreen.SetActive(true);
    }

    
}
