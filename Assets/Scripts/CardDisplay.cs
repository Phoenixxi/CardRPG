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
    public Button reshuffleButton;
    public Button costManipButton;
    
    void Start()
    {
        UpdateCardDisplay();
    }

    public void ResetEnergyDisplay()
    {
        int newEnergy = cardData.Energy + 2;
        cardData.Energy = newEnergy;
        energyText.text = newEnergy.ToString();
    }

    public void UpdateCardDisplay()
    {
        cardImage.sprite = cardData.cardSprite;
        energyText.text = cardData.Energy.ToString();
    }

    public void UpdateEnergyDisplayCostManip()
    {
            int newEnergy = cardData.Energy - 2;
            cardData.Energy = newEnergy;
            energyText.text = newEnergy.ToString();
        
    }



    public void UpdateEnergyText(int energyCost)
    {
        energyText.text = "Energy: " + energyCost.ToString();
    }
}
