using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardNamespace;

public class EnemyManager : MonoBehaviour
{
    private List<CardClickHandler> cardClickHandler = new List<CardClickHandler>();
    public GameObject attackVFX_1;
    Vector3 startVFXLocation = new Vector3(15.6000004f,3.74000001f,-15.71f);
    public HealthBar teamHealthBar;
     private HandManager handManager;
     public DeckManager deckManager;

    public bool EnemyTurn = false;

    public void SetCardClickHandler(CardClickHandler handler)
    {
        cardClickHandler.Add(handler);
    }

    public void EnemyTurnStart()
    {
        // Find the CardClickHandler in the scene if not assigned
        //cardClickHandler = FindObjectOfType<CardClickHandler>();

        if (cardClickHandler != null)
        {
            foreach(CardClickHandler handler in cardClickHandler){
                handler.ToggleEnemyTurn(true);
                Debug.Log("Enemy Turn Started - Cards Disabled");
            }
        }
        Invoke("EnemyAttack", 3f);
    }

    public void EnemyAttack()
    {
        GameObject vfxInstance = Instantiate(attackVFX_1, startVFXLocation, Quaternion.identity);

        ParticleSystem[] particleSystems = vfxInstance.GetComponentsInChildren<ParticleSystem>();

                // Play all particle systems
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Play();
        }
        // Destroy the VFX after it finishes playing, also play for 5 seconds
        Destroy(vfxInstance, 5f); 
        teamHealthBar.DecreaseTeamHealth(5);




        Invoke("EnemyTurnEnd", 3f);
    }

    public void EnemyTurnEnd()
    {
        if (cardClickHandler != null)
        {
            foreach(CardClickHandler handler in cardClickHandler){
                // enemy turn false
                handler.ToggleEnemyTurn(false);
            }
        }
        // empty card list for next turn
        cardClickHandler = new List<CardClickHandler>();
        handManager.ToggleBlackOverlay();
        deckManager.DrawTillFill(handManager);
    }


    public void SetHandManager(HandManager manager)
    {
        handManager = manager;
    }
}