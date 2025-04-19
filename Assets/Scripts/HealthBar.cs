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
