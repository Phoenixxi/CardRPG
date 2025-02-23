using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardNamespace;

public class AttackManager : MonoBehaviour
{
    public List<GameObject> cardsList = new List<GameObject>();
    public HealthBar healthBar;
    public EnemyHealthBar enemyHealthBar;
    public int currentHealth;

    void Start()
    {
        currentHealth = 100;
        healthBar.SetMaxHealth(100);
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

            enemyHealthBar.DecreaseEnemyHealth(cardDisplay.cardData.Damage);
        }

        EmptyCards();

    }

    public void UpdateHealth(int health){

    }

    public void EmptyCards(){
        cardsList = new List<GameObject>();
    }
}

