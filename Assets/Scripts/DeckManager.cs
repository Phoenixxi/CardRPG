using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardNamespace;

public class DeckManager : MonoBehaviour
{
   public List<Card> allCards = new List<Card>();
   private int currentIndex = 0;
   private HandManager handManager;

   void Start()
   {
        // Load all card assets from resources folder
      //  Card[] cards = Resources.LoadAll<Card>("Cards");

        // Add the loaded cards to the allCards list
       // allCards.AddRange(cards);

       handManager = FindObjectOfType<HandManager>();
       for(int i = 0; i < 6; i++)
       {
            DrawCard(handManager);
       }
   }
    public void DrawCard(HandManager handManager)
    {
        if(allCards.Count == 0)
            return;

        Card nextCard = allCards[currentIndex];
        handManager.AddCardToHand(nextCard);
        currentIndex = (currentIndex + 1) % allCards.Count;
        
    }

    public void DrawTillFill(HandManager handManger)
    {
        int currentCardAmount = handManager.cardsInHand.Count;
        while(currentCardAmount < 5)
        {
            Card nextCard = allCards[currentIndex];
            handManager.AddCardToHand(nextCard);
            currentIndex = (currentIndex + 1) % currentCardAmount;
            currentCardAmount = handManager.cardsInHand.Count;
        }

    }
}
