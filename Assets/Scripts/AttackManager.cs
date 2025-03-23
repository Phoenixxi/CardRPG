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
    private int BellaSpecialTurns = 0;
    [SerializeField] private Transform VFXSpawnPoint;
    [SerializeField] private Transform VFXImpactSpawn;

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
        healthBar.IncreaseTeamHealth(health);
        if(currentHealth + health >= healthBar.slider.maxValue)
        {
            currentHealth = healthBar.slider.maxValue;
            teamHealthCurrent.text = currentHealth.ToString();
        }
        else
        {
            currentHealth += health;
            teamHealthCurrent.text = currentHealth.ToString();
        }

    }
    
    public void AttackStart()
    {
        if(BellaSpecialTurns > 0)
        {
            BellaSpecialTurns--;
            // CHANGE 5 IF BELLA'S HEALING AMOUNT CHANGES FOR SPECIAL ABILITY
            IncreaseTeamHealth(5);
        }
        Vector3 startVFXLocation = new Vector3(6.04000006f, 2.529999995f ,-17.816000015f);

        // see if team buffs are being used
        float teamMultipler = 1f;
        foreach(GameObject card in cardsList){
            CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
            if(cardDisplay.cardData.cardType.ToString() == "TeamDmgMultiplier" || cardDisplay.cardData.cardType.ToString() == "BellaSpecial")
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
                GameObject vfxInstance = Instantiate(cardDisplay.cardData.vfxPrefab, VFXSpawnPoint.position, Quaternion.identity);
                vfxInstance.transform.localScale = VFXSpawnPoint.localScale;
                ParticleSystem[] particleSystems = vfxInstance.GetComponentsInChildren<ParticleSystem>();

                // Play all particle systems
                foreach (ParticleSystem ps in particleSystems)
                {
                    ps.Play();
                }

                // Destroy the VFX after it finishes playing, also play for 5 seconds
                Destroy(vfxInstance, 2f); 
                //yield return new WaitForSeconds(2f);

                if(cardDisplay.cardData.vfxImpact != null)
                {
                    GameObject vfxImpact = Instantiate(cardDisplay.cardData.vfxImpact, VFXImpactSpawn.position, Quaternion.identity);
                    vfxImpact.transform.localScale = VFXImpactSpawn.localScale;
                    ParticleSystem[] particleSystemImpact = vfxImpact.GetComponentsInChildren<ParticleSystem>();

                     foreach (ParticleSystem psi in particleSystemImpact)
                    {
                        psi.Play();
                    }
                    Destroy(vfxImpact, 2f); 
                }   
            }

            Card data = cardDisplay.cardData;
            string currentCardType = data.cardType.ToString();
            switch(currentCardType)
            {
                case "Damage":
                    enemyManager.DecreaseEnemyHealth(data.Damage * teamMultipler);
                    break;

                case "TeamHeal":
                    float health = data.Heal;
                    IncreaseTeamHealth(health);
                    break;

                case "DecEnemyDmg":
                    break;
                case "SingleAtkAdder":
                    enemyManager.DecreaseEnemyHealth(data.Single_atkAdder);
                    break;

                case "Shield":
                    enemyManager.ToggleSheildStatus(true);
                    break;

                case "DmgOverTime":
                    break;

                case "Thorns":
                    enemyManager.DecreaseEnemyHealth(data.Damage);
                    enemyManager.ToggleThornsStatus(true);
                    break;

                case "DiceManipulation":
                    handManager.DiceManipulationActive(data.DiceManipulationAmount);
                    break;

                case "KingFireBlastSpecial":
                    if(enemyManager.attackedLastTurn)
                        enemyManager.DecreaseEnemyHealth(data.Damage + 2);
                    else   
                        enemyManager.DecreaseEnemyHealth(data.Damage);
                    break;
                
                case "BellaSpecial":
                        BellaSpecialTurns = 2;
                        IncreaseTeamHealth(data.Heal);
                    break;
            }
        
        }

        EmptyCards();
        enemyManager.EnemyTurnStart();

    }

/*
    private IEnumerator PlayImpact(GameObject vfxImpact)
    {

    }
    */

    public void EmptyCards(){
        cardsList = new List<GameObject>();
    }
}

