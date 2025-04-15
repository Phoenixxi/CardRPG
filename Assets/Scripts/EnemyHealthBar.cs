using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CardNamespace;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;
    public Button victoryButton;
    

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
            victoryButton.gameObject.SetActive(true);
            slider.value = 0;
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
