using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using CardNamespace;
using Random = System.Random;

public class DeckManager : MonoBehaviour
{
   public List<Card> allCards = new List<Card>();
   public int currentIndex = 0;
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
       
            DrawTillFill(handManager);
            //DrawCard(handManager);
       
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

        while(currentCardAmount < 5)
        {
            Card nextCard = allCards[currentIndex];
            handManager.AddCardToHand(nextCard);
            currentIndex = (currentIndex + random.Next(0,allCards.Count)) % allCards.Count;
            currentCardAmount = handManager.cardsInHand.Count;
        }
    }
}
