using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CardNamespace;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public GameObject Loss;  
    public VictoryLossManager victoryLossManager;
     private int worldID;
    public NodeController activeNode;
    
    void Start()
    {
        activeNode = MapManager.Instance.nodes[0].sendCurrentNode();
        worldID = activeNode.thisWorld;

        if(worldID == 0)
        {   
            slider.maxValue = 50;
            slider.value = 50;
        }
        else if(worldID == 1 || worldID == 2)
        {
            slider.maxValue = 65;
            slider.value = 65;
        }
    }

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetTeamHealthSviur()
    {
        slider.value = slider.maxValue * .6f;
    }

     public float GetTeamMaxHealth()
    {
        return slider.maxValue;
    }
    
    public void DecreaseTeamHealth(float health)
    {
        // check if health will become 0 or less
        if(slider.value - health <= 0){
            slider.value = 0;
            victoryLossManager.winLossStatus = false;
            Loss.SetActive(true);
        }
        else{
            slider.value -= health;
        }
    }

    public void IncreaseTeamHealth(float health)
    {
        // check if health will become max
        if(slider.value + health >= slider.maxValue)
            slider.value = slider.maxValue;
        else
            slider.value += health;

    }
    
}
