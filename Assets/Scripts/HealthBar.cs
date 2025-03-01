using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CardNamespace;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
    
    public void DecreaseTeamHealth(float health)
    {
        // check if health will become 0 or less
        if(slider.value - health <= 0){}
            //end game
        
        slider.value -= health;
    }

    public void IncreaseTeamHealth(float health)
    {
        // check if health will become max
        if(slider.value + health >= slider.maxValue)
            slider.value = slider.maxValue;

        slider.value += health;

    }
    
}
