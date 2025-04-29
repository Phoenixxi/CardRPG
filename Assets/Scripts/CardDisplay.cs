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
        cardData.EnergyDecreasedBy = 0;
    }

    public void ResetEnergyDisplay()
    {
        int newEnergy = cardData.Energy + cardData.EnergyDecreasedBy;
        cardData.Energy = newEnergy;
        energyText.text = newEnergy.ToString();
        cardData.EnergyDecreasedBy = 0;
        
    }

    public void UpdateCardDisplay()
    {
        cardImage.sprite = cardData.cardSprite;
        energyText.text = cardData.Energy.ToString();
    }

    public void UpdateEnergyDisplayCostManip()
    {
        int newEnergy;
        if(cardData.Energy <= 2) // if <=2 energy, set to 0
        {
            newEnergy = 0;
        }
        else    // o.w. subtract 2
        {
            newEnergy = cardData.Energy - 2;
        }
         
        if(newEnergy <= 0 && cardData.Energy != 0){
            cardData.EnergyDecreasedBy += cardData.Energy;
            cardData.Energy = 0;
        }
        else if(newEnergy <= 0 && cardData.Energy == 0){
            cardData.Energy = 0;
        }
        else{
            cardData.EnergyDecreasedBy +=2;
            cardData.Energy = newEnergy;
        }

        energyText.text = newEnergy.ToString();
        
    }



    public void UpdateEnergyText(int energyCost)
    {
        energyText.text = "Energy: " + energyCost.ToString();
    }
}
