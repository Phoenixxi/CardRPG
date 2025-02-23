using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardNamespace;

public class EnemyManager : MonoBehaviour
{
    private List<CardClickHandler> cardClickHandler = new List<CardClickHandler>();


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
    }

    public void EnemyAttackStart()
    {

    }

    public void EnemyTurnEnd()
    {
        if (cardClickHandler != null)
        {
            foreach(CardClickHandler handler in cardClickHandler){
                handler.ToggleEnemyTurn(false);
                Debug.Log("Enemy Turn Ended - Player Can Interact Again");
            }
        }

        cardClickHandler = new List<CardClickHandler>();
    }
}