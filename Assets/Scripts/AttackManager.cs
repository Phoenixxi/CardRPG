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

    // Health bars
    public HealthBar healthBar;
    
    // Health bar text
    public int currentHealth;
    public Text teamHealthTotal;
    public Text teamHealthCurrent;

    void Start()
    {
        healthBar.SetMaxHealth(currentHealth);
        teamHealthTotal.text = currentHealth.ToString();
        teamHealthCurrent.text = currentHealth.ToString();
    }

    public void DecreaseTeamHealth(int health)
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

    public void IncreaseTeamHealth(int health)
    {

    }
    
    public void AttackStart()
    {
        Vector3 startVFXLocation = new Vector3(6.04000006f, 2.529999995f ,-17.816000015f);
        foreach(GameObject card in cardsList){

            CardDisplay cardDisplay = card.GetComponent<CardDisplay>();

            if (cardDisplay != null && cardDisplay.cardData != null && cardDisplay.cardData.vfxPrefab != null)
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

            enemyManager.DecreaseEnemyHealth(cardDisplay.cardData.Damage);
        }

        EmptyCards();
        enemyManager.EnemyTurnStart();

    }

    public void EmptyCards(){
        cardsList = new List<GameObject>();
    }
}

