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
    public GameObject diceRollButton;
    public GameObject diceBackgroundVFX;
    public GameObject diceRollVFX;
    public Button attackButton;
    [SerializeField] private Transform diceSpawnPoint;
    public Text resultText;     //shows in middle of screen after dice roll
    public Text energyText;    // displays the energy in the large pool
    public Text energyPoolText; // displays the energy in the small pool
    public int currentEnergy = 0;   // energy in the large pool
    public int energyPool = 0;      // energy in the small pool
    private bool costJustChanged = false;
    private bool DiceManipulationStatus = false;
    private int diceResult = 0;
    public GameObject enemyTurnObject; // active means it is enemy turn, inactive means it is not


    
    // Attack Managers
    public AttackManager attackManager;
    public EnemyManager enemyManager;
    private Vector3 startVFXLocation = new Vector3(6.04000006f, 2.529999995f ,-17.816000015f);
    public int diceEnergyAdder = 0;

    // Cost Manipulation
    List<string> cardNamesReset = new List<string>();
    List<string> cardNamesCostChange = new List<string>();
    private NodeController activeNode;
    private int worldID;

    public bool getCostJustChanged()
    {
        return costJustChanged;
    }
    void Start()
    {
        //diceBackgroundVFX.gameObject.transform.position = new Vector3(-953.320007f,-526.820007f,-6.92999983f);
        //diceBackgroundVFX.gameObject.transform.localScale = new Vector3(1.18471217f,1.18471217f,1.18471217f);
        activeNode = MapManager.Instance.nodes[0].sendCurrentNode();
        worldID = activeNode.thisWorld;

        blackOverlay.gameObject.SetActive(true);
        enemyTurnObject.gameObject.SetActive(false); // not enemy's turn at start
        resultText.text = "";
        energyText.text = "0";
        energyPoolText.text = "0";
        energyText.text = currentEnergy.ToString();
    }

    public void RollDice()
    {
        diceBackgroundVFX.gameObject.SetActive(false);

        int upperRange, lowerRange;
        // Set dice range based on world
        if(worldID == 0)
        {
            lowerRange = 2;
            upperRange = 11;
        }
        else
        {
            lowerRange = 5; 
            upperRange = 19;
        }
            

        // Call with button
        // Random number between 1-10
        if(!DiceManipulationStatus)         // check if dice has been manipulated
            diceResult = Random.Range(lowerRange, upperRange); 
        DiceManipulationStatus = false;     // set back to false


        // TEMPORARY MAKE GO AWAY LATER
        diceResult = 20;
        diceResult += diceEnergyAdder;
        
        currentEnergy = diceResult + energyPool;

        blackOverlay.gameObject.SetActive(false);
        
        StartCoroutine(ShowResult(diceResult));
        diceRollButton.gameObject.SetActive(false);
        diceEnergyAdder = 0;    // set back to 0
    }

    public void DiceManipulationActive(int amount)
    {
        int upperRange;
        // Set dice range based on world
        if(worldID == 0)
            upperRange = 11;
        else
            upperRange = 19;
        

        DiceManipulationStatus = true;
        diceResult = Random.Range(amount, upperRange);
    }

    public void ToggleBlackOverlay()
    {
        blackOverlay.gameObject.SetActive(true);
       
    }

    public void ToggleDiceButton()
    {
        diceRollButton.gameObject.SetActive(true);
        diceBackgroundVFX.gameObject.SetActive(true);
        SetAttackButtonActive(true);
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
        if((diceResult + energyPool) >= 3)
        {
          int bigEnergy = (diceResult + energyPool) - 3;
          int smallEnergy = (diceResult + energyPool) - bigEnergy;
          currentEnergy = bigEnergy;
          energyText.text = bigEnergy.ToString();
          energyPoolText.text = smallEnergy.ToString();
          energyPool = smallEnergy;
        }
        else
        {
            int temp = (diceResult + energyPool);
            energyText.text = "0";
            currentEnergy = temp;
            energyPoolText.text = temp.ToString();
            energyPool = temp;
        }
        resultText.text = diceResult.ToString();
        //energyText.text = diceResult.ToString();

         // Turn off "enemy turn"
        enemyTurnObject.gameObject.SetActive(false);
        
        // Clear temporary result and update Energy text
        yield return new WaitForSeconds(1f);
        resultText.text = "";
        //UpdateEnergyDisplay();
    }

    public void AddCardToHand(Card cardData)
    {
        if(cardsInHand.Count < maxHandSize){
        // Instantiate card Game Object
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

    public void SetAttackButtonActive(bool active)
    {
        attackButton.interactable = active;
    }

    public void Attack()
    {
        SetAttackButtonActive(false);
        List<GameObject> cardsToRemove = new List<GameObject>();

        // Reset cost manipulation amount
        foreach(GameObject card in cardsToRemove)
        {
            CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
            cardDisplay.ResetEnergyDisplay();
        }
            
        // Identify selected cards
        foreach (GameObject card in cardsInHand)
        {
            CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
            if(costJustChanged){
                cardDisplay.ResetEnergyDisplay();
            }
            
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
        cardNamesReset = new List<string>();

        currentEnergy = 0;
        energyText.text = "0";

        // Toggle on "enemy turn"
        enemyTurnObject.gameObject.SetActive(true);
        // Send all information to attack manager
        attackManager.AttackStart();
    }

    public void ReshuffleCards(){

        List<Card> cardsToSave = new List<Card>();
        bool handLock = false;

        List<GameObject> cardsToRemove = new List<GameObject>();
        foreach(GameObject card in cardsInHand)
        {
            if((card.GetComponent<CardDisplay>().cardData.Reshuffle || card.GetComponent<CardDisplay>().cardData.ReshuffleElio) && !handLock)
            {
                handLock = true;
                cardsToRemove.Add(card);
                continue;
            }
               
            cardsToSave.Add(card.GetComponent<CardDisplay>().cardData);
            cardsToRemove.Add(card);
        }

        foreach(GameObject card in cardsToRemove){
            cardsInHand.Remove(card);
            Destroy(card);
        }
        
        deckManager.DrawAfterReshuffle(this, cardsToSave);
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

        foreach(GameObject card in cardsInHand)
        {
            CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
            if(!cardNamesCostChange.Contains(cardDisplay.cardData.cardName)){
                cardNamesCostChange.Add(cardDisplay.cardData.cardName);
                
                if(cardDisplay.cardData.CostManipulation){
                    cardsToRemove.Add(card);
                    cardNamesReset.Add(cardDisplay.cardData.cardName);
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

        cardNamesCostChange = new List<string>();
        costJustChanged = true;
        UpdateHandVisuals();

    }
  

    public void UpdateEnergyDisplay(int newEnergy)
    {
        Debug.Log("NEW ENERGY: " + newEnergy);
        if((newEnergy) >= 3)
        {
          int bigEnergy = (newEnergy) - 3;
           Debug.Log("BIG ENERGY: " + bigEnergy);
          int smallEnergy = (newEnergy) - bigEnergy;
          currentEnergy = bigEnergy;
          energyText.text = bigEnergy.ToString();
            Debug.Log("big energy text " + energyText.text);
          energyPoolText.text = smallEnergy.ToString();
          energyPool = smallEnergy;
        }
        else
        {
            int temp = (newEnergy);
            energyText.text = "0";
            currentEnergy = temp;
            energyPoolText.text = temp.ToString();
            energyPool = temp;
        }
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
       energyText.text = currentEnergy.ToString();
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



