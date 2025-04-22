using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CardNamespace;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;

    public GameObject VictoryEnemy;
    public GameObject VictorySviur;
    public GameObject VictoryEstella;
    public GameObject VictoryBigBoss;

    public VictoryLossManager victoryLossManager;

    private int worldID;
    private bool isBossBattle;
    
    void Start()
    {
        NodeController activeNode = MapManager.Instance.nodes[0].sendCurrentNode();
        worldID = activeNode.thisWorld;
        isBossBattle = activeNode.isBossNode;
    } 

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public float GetEnemyMaxHealth()
    {
        return slider.maxValue;
    }

    public void SetEnemyHealth()
    {
        slider.value = slider.maxValue * .6f;
    }
    
    public void DecreaseEnemyHealth(float health)
    {
        // check if health will become 0 or less
        if(slider.value - health <= 0){
            // Go to victory/loss scene
            slider.value = 0;
            victoryLossManager.winLossStatus = true;
            Debug.Log("status is: " + victoryLossManager.winLossStatus);
            // not a boss battle
            if(!isBossBattle)
                VictoryEnemy.SetActive(true);
            else if(isBossBattle && worldID == 0)
                VictorySviur.SetActive(true);
            else if(isBossBattle && worldID == 1)
                VictoryEstella.SetActive(true);
            else if(isBossBattle && worldID == 2)
                VictoryBigBoss.SetActive(true);
        }
        else{
            slider.value -= health;
        }
        
    }

    public void IncreaseEnemyHealth(float health)
    {
        // check if health will become max
        if(slider.value + health >= slider.maxValue)
            slider.value = slider.maxValue;
            
        slider.value += health;

    }

}
