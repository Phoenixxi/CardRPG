using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CardNamespace;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
    public void DecreaseEnemyHealth(int health)
    {
        // check if health will become 0 or less
        if(slider.value - health <= 0){}
            //end game
        
        slider.value -= health;
    }

    public void IncreaseEnemyHealth(int health)
    {
        // check if health will become max
        if(slider.value + health >= slider.maxValue)
            slider.value = slider.maxValue;
            
        slider.value += health;

    }
}
