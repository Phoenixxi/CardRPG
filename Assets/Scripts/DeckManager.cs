using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using CardNamespace;
using Random = System.Random;
using System.Linq;

public class DeckManager : MonoBehaviour
{
   public List<Card> allCards = new List<Card>();
   private int currentIndex = 0;
   public SpriteRenderer thirdCharacterSplash;
   public SpriteRenderer thirdCharacterCard;
   public List<Sprite> splashSpriteList;
   public List<Sprite> cardSpriteList;
   private HandManager handManager;
   public List<GameObject> cardsToFill;
   private Random random = new System.Random();

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
         // Get characters from deck builder
        int[] characterSelected = (DeckScreenManager.Instance.RN_CharacterScreenManager.sendCharacterID());

       // UPDATE AFTER VS
        for(int i = 0; i < characterSelected.Count(); i++){
            if(characterSelected[i] == 1 || characterSelected[i] == 2 || characterSelected[i] == 0)
                continue;
            else if(characterSelected[i] == 3) // Bella
            {
                thirdCharacterSplash.sprite = splashSpriteList[0];
                thirdCharacterCard.sprite = cardSpriteList[0];
            }
            else if(characterSelected[i] == 4) // King Fire Blast
            {
                thirdCharacterSplash.sprite = splashSpriteList[1];
                thirdCharacterCard.sprite = cardSpriteList[1];
            }
        }   
        
        
       
       
            DrawTillFill(handManager);
            //DrawCard(handManager);
       
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

    public void DrawTillFill(HandManager handManger)
    {
        int currentCardAmount = handManager.cardsInHand.Count;
        currentIndex = random.Next(0, allCards.Count);

        while(currentCardAmount < 5 && allCards.Count > 0)
        {
             // get new index
            currentIndex = (currentIndex + random.Next(0,allCards.Count)) % allCards.Count;

            Card nextCard = allCards[currentIndex];
            handManager.AddCardToHand(nextCard);
            // remove card instance
            //allCards.RemoveAt(currentIndex);

            currentCardAmount = handManager.cardsInHand.Count;
           
        }
    }
}
