using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CardNamespace;

public class CardDisplay : MonoBehaviour
{
    public Card cardData;
    public Image cardImage;
    public Text energyText; 
    
    void Start()
    {
        UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
    {
        cardImage.sprite = cardData.cardSprite;
        energyText.text = cardData.Energy.ToString();
    }


    public void UpdateEnergyText(int energyCost)
    {
        energyText.text = "Energy: " + energyCost.ToString();
    }
}
