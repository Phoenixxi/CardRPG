using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CardNamespace;
using UnityEngine.VFX;
using UnityEngine.SceneManagement;
//using System;



public class HandManager : MonoBehaviour
{
    //public DeckManager deckManager;

    // Current Hand fields
    public GameObject cardPrefab;
    public Transform handTransform; // root of hand position
    public float fanSpread = -7.5f;    // angle of cards
    public float cardSpacing = 160f;  // how much hand will spread out
    public float verticalSpacing = 53f; // pull cards on sides down
    public int maxHandSize = 5;
    public List<GameObject> cardsInHand = new List<GameObject>(); // holds cards
    public AttackManager attackManager;
    public EnemyManager enemyManager;
    public GameObject blackOverlay;
   

    // Stuff for dice
     public Text resultText; 
    public Text energyText; 
     public int currentEnergy = 0; 


    void Start()
    {
        blackOverlay.gameObject.SetActive(true);
        resultText.text = "";
        energyText.text = "0";
        UpdateEnergyDisplay();
    }

    public void ToggleBlackOverlay()
    {
        blackOverlay.gameObject.SetActive(true);
    }

    public void RollDice()
    {
        blackOverlay.gameObject.SetActive(false);
        // Call with button
        // Random number between 1-10
        int diceResult = Random.Range(3, 11); 
        //int diceResult = 10;
        currentEnergy = diceResult;
        StartCoroutine(ShowResult(diceResult));

    }
     private IEnumerator ShowResult(int diceResult)
    {
        // temporary for prototype before we get roll animation
        resultText.text = "Rolling...";
        yield return new WaitForSeconds(1.5f);
        resultText.text = diceResult.ToString();
        energyText.text = diceResult.ToString();

        // Clear temporary result and update Energy text
        yield return new WaitForSeconds(1f);
        resultText.text = "";
        UpdateEnergyDisplay();
    }

    public void AddCardToHand(Card cardData)
    {
        if(cardsInHand.Count < maxHandSize){
        // Instantiate card
        GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
        // Add new card
        cardsInHand.Add(newCard);

        // Set card data of instantiated card
        newCard.GetComponent<CardDisplay>().cardData = cardData;

        CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();
        cardDisplay.cardData = cardData;

        // Assign HandManager to CardClickHandler for this card
        CardClickHandler clickHandler = newCard.GetComponent<CardClickHandler>();
            if (clickHandler != null)
            {
                clickHandler.SetHandManager(this);
                enemyManager.SetHandManager(this);

                // set card's CCH in enemyManager
                enemyManager.SetCardClickHandler(clickHandler);
            }  
        }

        // Update hand on screen
        UpdateHandVisuals();
    }

    public void Attack()
    {
        List<GameObject> cardsToRemove = new List<GameObject>();

        // Identify selected cards
        foreach (GameObject card in cardsInHand)
        {
            CardClickHandler clickHandler = card.GetComponent<CardClickHandler>();
            if (clickHandler != null && clickHandler.IsSelected())
            {
                attackManager.cardsList.Add(card);
                cardsToRemove.Add(card);
            }
        }
        
        // Remove selected cards
        foreach (GameObject card in cardsToRemove)
        {
            cardsInHand.Remove(card);
            Destroy(card); 
        }

        // Update visuals after removal
        UpdateHandVisuals();

        // Send all information to attack manager
        attackManager.AttackStart();
    }

    public void UpdateScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

    public void UpdateEnergyDisplay()
    {
        energyText.text = currentEnergy.ToString();
    }

    public void UpdateEnergy(int energyCost, bool isSelected)
    {
        if (isSelected)
        {
            currentEnergy -= energyCost;  // Subtract energy when selected
        }
        else
        {
            currentEnergy += energyCost;  // Add energy back when deselected
        }

        // Update the energy display
        UpdateEnergyDisplay();
    }

    void Update()
    {
         //UpdateHandVisuals();
    }

    public void UpdateHandVisuals()
    {
        int cardCount = cardsInHand.Count;

        // If only 1 card is in hand, it wont send null
        if(cardCount == 1)
        {
            cardsInHand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            cardsInHand[0].transform.localPosition = new Vector3(0f, 0f, 0f);
            return;
        }
        for(int i = 0; i < cardCount; i++)
        {
            // Rotation angle
            float rotationAngle = (fanSpread * (i - (cardCount - 1) / 2f));
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);

            // Horizontal and Vertical offset of cards
            float horizontalOffset = (cardSpacing * (i - (cardCount - 1) / 2f));

            // Normalize card position between -1 and 1
            float normalizePosition = (2f * i / (cardCount - 1) - 1f);
            float verticalOffset = verticalSpacing * (1 - normalizePosition * normalizePosition);

            // Set card positions
            cardsInHand[i].transform.localPosition = new Vector3(horizontalOffset, verticalOffset, 0f);

        }

        foreach(GameObject card in cardsInHand){
            CardClickHandler clickHandler = card.GetComponent<CardClickHandler>();
            clickHandler.UpdateCardPositions();
        }
       
    }


}
