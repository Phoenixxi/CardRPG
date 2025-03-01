using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CardNamespace;

public class AttackManager : MonoBehaviour
{
    // Managers
    public List<GameObject> cardsList = new List<GameObject>();
    public EnemyManager enemyManager;
    public HandManager handManager;

    // Health bars
    public HealthBar healthBar;
    
    // Health bar text
    public float currentHealth;
    public Text teamHealthTotal;
    public Text teamHealthCurrent;

    void Start()
    {
        healthBar.SetMaxHealth(currentHealth);
        teamHealthTotal.text = currentHealth.ToString();
        teamHealthCurrent.text = currentHealth.ToString();
    }

    public void DecreaseTeamHealth(float health)
    {
        // If player dies
        if(currentHealth - health <= 0){
            teamHealthCurrent.text = "0";
            healthBar.DecreaseTeamHealth(health);
        }
        else
        {
            currentHealth -= health;
            teamHealthCurrent.text = currentHealth.ToString();
            healthBar.DecreaseTeamHealth(health);
        }
    }

    public void IncreaseTeamHealth(float health)
    {

    }
    
    public void AttackStart()
    {
        Vector3 startVFXLocation = new Vector3(6.04000006f, 2.529999995f ,-17.816000015f);

        // see if team buffs are being used
        float teamMultipler = 1f;
        foreach(GameObject card in cardsList){
            CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
            if(cardDisplay.cardData.cardType.ToString() == "TeamDmgMultiplier")
                teamMultipler = cardDisplay.cardData.Team_dmgMultiplier;
        }


        // incoming dmg reducer sends signal to enemy manager, handled over there during enemy turn (field?)
        foreach(GameObject card in cardsList){

            CardDisplay cardDisplay = card.GetComponent<CardDisplay>();

            // use cardDisplay.character to get character position so VFX can play from the correct place
            if (cardDisplay.cardData.vfxPrefab != null)  //cardDisplay != null && cardDisplay.cardData != null && 
            {
                // Instantiate VFX at the attack location
                // NOTE: NEED TO CHANGE VECTOR 3 LOCATION TO LOCATION OF CHARACTER
                GameObject vfxInstance = Instantiate(cardDisplay.cardData.vfxPrefab, startVFXLocation, Quaternion.identity);

                ParticleSystem[] particleSystems = vfxInstance.GetComponentsInChildren<ParticleSystem>();

                // Play all particle systems
                foreach (ParticleSystem ps in particleSystems)
                {
                    ps.Play();
                }

                // Destroy the VFX after it finishes playing, also play for 5 seconds
                Destroy(vfxInstance, 5f); 
            }

            Card data = cardDisplay.cardData;
            string currentCardType = data.cardType.ToString();
            switch(currentCardType)
            {
                case "Damage":
                    enemyManager.DecreaseEnemyHealth(data.Damage * teamMultipler);
                    break;

                case "TeamHeal":
                    healthBar.IncreaseTeamHealth(data.Heal);
                    break;

                case "DecEnemyDmg":
                    break;
                case "SingleAtkAdder":
                    enemyManager.DecreaseEnemyHealth(data.Single_atkAdder);
                    break;

                case "Shield":
                    break;

                case "DmgOverTime":
                    break;
                case "Thorns":
                    break;
                case "DiceManipulation":
                    break;
            }
        


            
        }

        EmptyCards();
        enemyManager.EnemyTurnStart();

    }

    public void EmptyCards(){
        cardsList = new List<GameObject>();
    }
}

