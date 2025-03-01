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
    public DeckManager deckManager;

    // Current Hand fields
    public GameObject cardPrefab;
    public List<GameObject> cardsInHand = new List<GameObject>(); // holds cards

    // Hand movement
    public Transform handTransform; // root of hand position
    public float fanSpread = -7.5f;    // angle of cards
    public float cardSpacing = 160f;  // how much hand will spread out
    public float verticalSpacing = 53f; // pull cards on sides down
    public int maxHandSize = 5;
    private float hoverOffset = 70f;

    // Dice / Energy UI
    public GameObject blackOverlay;
    public GameObject diceRollVFX;
    [SerializeField] private Transform diceSpawnPoint;
    public Text resultText; 
    public Text energyText; 
    public int currentEnergy = 0; 
    private bool costJustChanged = false;
    
    // Attack Managers
    public AttackManager attackManager;
    public EnemyManager enemyManager;
    private Vector3 startVFXLocation = new Vector3(6.04000006f, 2.529999995f ,-17.816000015f);



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
        int diceResult = Random.Range(6, 11); 
        //int diceResult = 10;
        currentEnergy = diceResult;
        StartCoroutine(ShowResult(diceResult));

    }
     private IEnumerator ShowResult(int diceResult)
    {
        //roll animation
        GameObject rollDiceVFX = Instantiate(diceRollVFX, diceSpawnPoint.position, diceSpawnPoint.rotation);
        rollDiceVFX.transform.localScale = diceSpawnPoint.localScale;
        ParticleSystem[] particleSystems = rollDiceVFX.GetComponentsInChildren<ParticleSystem>();

        // Play all particle systems
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Play();
        }
        // Destroy the VFX after it finishes playing, also play for 5 seconds
        Destroy(rollDiceVFX, 3f); 


        yield return new WaitForSeconds(1f);
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
        List<string> cardNames = new List<string>();

        // Identify selected cards
        foreach (GameObject card in cardsInHand)
        {
            CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
            if(costJustChanged && !cardNames.Contains(cardDisplay.cardData.cardName)){
                cardNames.Add(cardDisplay.cardData.cardName);
                cardDisplay.ResetEnergyDisplay();
            }
            else if(costJustChanged && cardNames.Contains(cardDisplay.cardData.cardName))
                cardDisplay.UpdateCardDisplay();
            
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
        costJustChanged = false;
        cardNames = new List<string>();

        // Send all information to attack manager
        attackManager.AttackStart();
    }

    public void ReshuffleCards(){

        // CHANGE THIS TO RESHUFFLE CARD ENERGY AMOUNT
        if(currentEnergy < 1)
            return;

        List<GameObject> cardsToRemove = new List<GameObject>();
        foreach(GameObject card in cardsInHand)
            cardsToRemove.Add(card);

        foreach(GameObject card in cardsToRemove){
            cardsInHand.Remove(card);
            Destroy(card);
        }
        
        // CHANGE THIS TO RESHUFFLE CARD ENERGY AMOUNT
        currentEnergy -= 1;
        energyText.text = currentEnergy.ToString();
        deckManager.DrawTillFill(this);
    }

    public void CostManipulationDisplayUpdate()
    {
        // check if cards are selected
        foreach(GameObject card in cardsInHand)
        {
            CardClickHandler clickHandler = card.GetComponent<CardClickHandler>();
            if(clickHandler.IsSelected()){
                Debug.Log("Unselect cards first!");
                return;
            }
        }

        List<GameObject> cardsToRemove = new List<GameObject>();
        List<string> cardNames = new List<string>();

        foreach(GameObject card in cardsInHand)
        {
            CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
            if(!cardNames.Contains(cardDisplay.cardData.cardName)){
                cardNames.Add(cardDisplay.cardData.cardName);
                
                if(cardDisplay.cardData.CostManipulation){
                    cardsToRemove.Add(card);
                    continue;
                }
                cardDisplay.UpdateEnergyDisplayCostManip();
            }
            else
            {
                cardDisplay.UpdateCardDisplay();
            }
            
            
        }
        // Get rid of cost manipulation card in hand
        foreach(GameObject card in cardsToRemove){
            cardsInHand.Remove(card);
            Destroy(card);
        }

        cardNames = new List<string>();
        costJustChanged = true;
        UpdateHandVisuals();

    }

    public void UpdateScene()
    {
        //FIX THIS DIRECTORY
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

    public void AdjustHandForHoveredCard(CardClickHandler hoveredCard)
    {
        int cardCount = cardsInHand.Count;
        if (cardCount <= 1) return;

        int hoveredIndex = cardsInHand.IndexOf(hoveredCard.gameObject);
        if (hoveredIndex == -1) return;

        for (int i = 0; i < cardCount; i++)
        {
            GameObject card = cardsInHand[i];
            CardClickHandler clickHandler = card.GetComponent<CardClickHandler>();
            Vector3 pos = clickHandler.GetOGPosition();
            Vector3 selectedPos = card.transform.localPosition;
                
            if(i < hoveredIndex && clickHandler.IsSelected())
            {
                card.transform.localPosition = selectedPos + new Vector3(-hoverOffset, 0, 0);
                //StartCoroutine(SmoothMove(card, newPos, hoverMoveDuration));
            }
            else if(i > hoveredIndex && clickHandler.IsSelected())
            {
                card.transform.localPosition = selectedPos + new Vector3(hoverOffset, 0, 0);
                //StartCoroutine(SmoothMove(card, newPos, hoverMoveDuration));
            }
            else if (i < hoveredIndex) // Move left-side cards left
            {
                card.transform.localPosition = pos + new Vector3(-hoverOffset, 0, 0);
                //StartCoroutine(SmoothMove(card, newPos, hoverMoveDuration));
            }
            else if (i > hoveredIndex) // Move right-side cards right
            {
                card.transform.localPosition = pos + new Vector3(hoverOffset, 0, 0);
                //StartCoroutine(SmoothMove(card, newPos, hoverMoveDuration));
            }
            
        }
    }

    public void ResetHandPositions(CardClickHandler hoveredCard)
    {
        int cardCount = cardsInHand.Count;
        if (cardCount <= 1) return;

        int hoveredIndex = cardsInHand.IndexOf(hoveredCard.gameObject);
        if (hoveredIndex == -1) return;

        for (int i = 0; i < cardCount; i++)
        {
            GameObject card = cardsInHand[i];
            CardClickHandler clickHandler = card.GetComponent<CardClickHandler>();
            Vector3 pos = clickHandler.GetOGPosition();
            Vector3 selectedPos = clickHandler.GetOGSelectedPosition();
            
            

            if(i < hoveredIndex && clickHandler.IsSelected())
            {
                card.transform.localPosition = selectedPos;
            }
            else if(i > hoveredIndex && clickHandler.IsSelected())
            {
                card.transform.localPosition = selectedPos;
            }
            else if (i < hoveredIndex) // Move left-side cards left
            {
                card.transform.localPosition = pos;
               // StartCoroutine(SmoothMove(card, pos, hoverMoveDuration));
            }
            else if (i > hoveredIndex) // Move right-side cards right
            {
                card.transform.localPosition = pos;
                //StartCoroutine(SmoothMove(card, pos, hoverMoveDuration));
            }
        }


       // UpdateHandVisuals(); // Reset all cards back to normal positions
    }

    // Smoothly moves a card to the target position
    private IEnumerator SmoothMove(GameObject card, Vector3 targetPos, float duration)
    {
        float elapsed = 0f;
        Vector3 startPos = card.transform.localPosition;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            card.transform.localPosition = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            yield return null;
        }

        card.transform.localPosition = targetPos;
    }
}



