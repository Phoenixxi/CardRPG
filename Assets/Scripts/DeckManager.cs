using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using CardNamespace;
using Random = System.Random;
using System.Linq;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
   public List<Card> allCards = new List<Card>();
   private int currentIndex = 0;
   public Button lossButton;
   public GameObject combatSceneW1;
   public GameObject combatSceneW2;
   public GameObject combatSceneW3;
   public SpriteRenderer topCharacterSplash;
   public SpriteRenderer topCharacterCard;
   public SpriteRenderer middleCharacterSplash;
   public SpriteRenderer middleCharacterCard;
   public SpriteRenderer bottomCharacterSplash;
   public SpriteRenderer bottomCharacterCard;

// NEED TO PROGRAM
   public SpriteRenderer enemyCharacterSplash;
   public SpriteRenderer enemyCharacterCard;
   public List<Sprite> splashSpriteList;
   public List<Sprite> cardSpriteList;
   public List<Sprite> enemySpriteList;
   private HandManager handManager;
   public List<GameObject> cardsToFill;
   private Random random = new System.Random();
   private int[] characterSelected;

   void Start()
   {
        // Load all card assets from resources folder
      //  Card[] cards = Resources.LoadAll<Card>("Cards");

        // Add the loaded cards to the allCards list
       // allCards.AddRange(cards);
        currentIndex = random.Next(0, allCards.Count);
       handManager = FindObjectOfType<HandManager>();
        
        // Get cards from deck builder
         allCards = DeckScreenManager.Instance.RN_DeckScreenManager.sendDeck();

         int cardsUnlocked = DeckScreenManager.Instance.RN_CharacterScreenManager.sendNumberOfCharacters();

         if(cardsUnlocked == 2){
            characterSelected = DeckScreenManager.Instance.RN_CharacterScreenManager.sendCharacterIDTwo();
         }else{
            characterSelected = DeckScreenManager.Instance.RN_CharacterScreenManager.sendCharacterIDThree();
         }

         /* TO DO */
         // Get world scene number and turn that asset on

       // ORDER [middle, bottom, top]
        //      [   0  ,   1   ,  2 ]

        // Get the ID of the character, and search them in the sprite list
        middleCharacterSplash.sprite = splashSpriteList[characterSelected[0]];
        middleCharacterCard.sprite = cardSpriteList[characterSelected[0]];
        int charIDmiddle = characterSelected[0];

        bottomCharacterSplash.sprite = splashSpriteList[characterSelected[1]];
        bottomCharacterCard.sprite = cardSpriteList[characterSelected[1]];
         int charIDbottom = characterSelected[1];

        bool threeCharacters = false;
        int charIDtop = -1;
        if(characterSelected.Count() == 3)
        {
            topCharacterSplash.sprite = splashSpriteList[characterSelected[2]];
            topCharacterCard.sprite = cardSpriteList[characterSelected[2]];
            charIDtop = characterSelected[2];
            threeCharacters = true;
        }

        // Assign the card positions (for VFX) based on where the characters are on screen
        foreach(Card card in allCards)
        {
            if(card.characterID == charIDmiddle)
                card.CharacterPosition = "middle";
            else if(card.characterID == charIDbottom)
                card.CharacterPosition = "bottom";
            else if(threeCharacters && card.characterID == charIDtop)
                card.CharacterPosition = "top";
        }
       
        DrawTillFill(handManager);
   }

   public void TutorialHelper()
   {

   }
    public void DrawCard(HandManager handManager)
    {
        if(allCards.Count == 0)
            return;

        Card nextCard = allCards[currentIndex];
        handManager.AddCardToHand(nextCard);
        currentIndex = (currentIndex + 1) % allCards.Count;
        
    }

    public void SetCardsToFill(List<GameObject> list)
    {
        cardsToFill = list;
    }

    public void DrawAfterReshuffle(HandManager handManager, List<Card> addBackToAllCards)
    {
        int currentCardAmount = handManager.cardsInHand.Count;

        // Give back the reshuffled cards
        foreach(Card card in addBackToAllCards)
            allCards.Add(card);

        currentIndex = random.Next(0, allCards.Count);
        Debug.Log("num of cards: " + allCards.Count);

        if(allCards.Count == 0 && currentCardAmount == 0)
        {
            lossButton.gameObject.SetActive(true);
            return;
        }

        while(currentCardAmount < 5 && allCards.Count > 0)
        {
             // get new index
            currentIndex = (currentIndex + random.Next(0,allCards.Count)) % allCards.Count;

            Card nextCard = allCards[currentIndex];
            handManager.AddCardToHand(nextCard);
            // remove card instance
            allCards.RemoveAt(currentIndex);

            currentCardAmount = handManager.cardsInHand.Count;
           
        }
    }

    public void DrawTillFill(HandManager handManger)
    {
        int currentCardAmount = handManager.cardsInHand.Count;
        currentIndex = random.Next(0, allCards.Count);

        Debug.Log("num of cards: " + allCards.Count);

        if(allCards.Count == 0 && currentCardAmount == 0)
        {
            lossButton.gameObject.SetActive(true);
            return;
        }

        while(currentCardAmount < 5 && allCards.Count > 0)
        {
             // get new index
            currentIndex = (currentIndex + random.Next(0,allCards.Count)) % allCards.Count;

            Card nextCard = allCards[currentIndex];
            handManager.AddCardToHand(nextCard);
            // remove card instance
            allCards.RemoveAt(currentIndex);

            currentCardAmount = handManager.cardsInHand.Count;
           
        }
    }
}
