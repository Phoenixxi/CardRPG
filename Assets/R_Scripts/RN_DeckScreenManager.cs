using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CardNamespace;

public class RN_DeckScreenManager : MonoBehaviour
{
    public List<RN_Card> Deck = new List<RN_Card>();
    private GameObject CardLayoutGroup1;
    private GameObject CardLayoutGroup2;
    private GameObject CardLayoutGroup3;
    private GameObject DeckList;
    private List<RN_Card> DisplayList = new List<RN_Card>();
    [SerializeField]private GameObject CardPrefab;
    [SerializeField]public UnityEngine.UI.Button ContinueButton;
    [SerializeField]public UnityEngine.UI.Button BackButton;
    
    public void Initialize()
    {
        CardLayoutGroup1 = GameObject.Find("CardLayoutGroup1");
        CardLayoutGroup2 = GameObject.Find("CardLayoutGroup2");
        CardLayoutGroup3 = GameObject.Find("CardLayoutGroup3");
        DeckList = GameObject.Find("DeckList");
        ContinueButton.interactable = false;
    }

    public void DisplayCards(RN_CharacterCard[] characters)
    {
        for (int i = 0; i < characters.Count(); i++)
        {
            if(characters[i] == null)
            {
                continue;
            }
            RN_CharacterCard character = characters[i];
            foreach(Card card in character.characterCard.cards)
            {
                GameObject CCard = Instantiate(CardPrefab);
                RN_Card CCard_script = CCard.GetComponent<RN_Card>();
                CCard_script.card = card;
                CCard_script.Resize();
                // Prevent multiple event subscriptions
                CCard_script.OnCardSelected -= CardClicked;
                CCard_script.OnCardSelected += CardClicked;

                switch(i)
                {
                    case 0:
                        CCard.transform.SetParent(CardLayoutGroup1.transform);
                        break;
                    case 1:
                        CCard.transform.SetParent(CardLayoutGroup2.transform);
                        break;
                    case 2:
                        CCard.transform.SetParent(CardLayoutGroup3.transform);
                        break;
                }

                DisplayList.Add(CCard_script);
            }
        }
    }

    public void CardClicked(RN_Card card)
    {
        //CASE for when the player wants to remove a card
        if(Deck.Contains(card))
        {
            Deck.Remove(card);
            card.gameObject.SetActive(false);
            Destroy(card.gameObject);
            ContinueButton.interactable = false;

            //Update the UI that displays how many cards of that type is in the deck
            RN_Card displayCard = DisplayList.Find(x => x.card.cardName == card.card.cardName && x.card.character == card.card.character);
            displayCard.removeToCount();
            return;
        }

        //CASE for when the deck has already hit 20 cards
        if(Deck.Count >= 20)
        {
            return;
        }

        //CASE for when there is already 4 cards of a certain card in the deck
        if(Deck.FindAll(n => n.card == card.card).Count >= 4)
        {
            return;
        }
        //CASE for when the player wants to add a card
        else
        {
            //Create the card
            GameObject CCard = Instantiate(CardPrefab);
            RN_Card CCard_script = CCard.GetComponent<RN_Card>();
            CCard_script.card = card.card;
            CCard_script.Resize();

            // Prevent multiple event subscriptions
            CCard_script.OnCardSelected -= CardClicked;
            CCard_script.OnCardSelected += CardClicked;

            //The CardPrefab has a text child component attached to it
            //We do not want that text child component when the card is being added to the deck, so we remove it
            foreach(Transform child in CCard.transform)
            {
                child.gameObject.SetActive(false);
            }

            CCard.transform.SetParent(DeckList.transform);
            //For formatting concerns, we want the same type of card to be displayed next to each other
            foreach(RN_Card oc in Deck)
            {
                if(oc.card == CCard_script.card)
                {
                    CCard.transform.SetSiblingIndex(oc.gameObject.transform.GetSiblingIndex()+1);
                    break;
                }
            }

            //Add the card to the deck
            Deck.Add(CCard_script);

            //Check if the deck has 20 cards
            if(Deck.Count == 20)
            {
                ContinueButton.interactable = true;
            }

            //Update the UI to display the amount of cards of that type
            card.addToCount();
        }
    }

    public List<Card> sendDeck(){
        List<Card> actualCards = new List<Card>();
        foreach(RN_Card card in Deck){
            actualCards.Add(card.card);
        }
        return actualCards;
    }

    public void reset()
    {
        foreach(Transform child in CardLayoutGroup1.transform)
        {
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);
        }

        foreach(Transform child in CardLayoutGroup2.transform)
        {
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);
        }

        foreach(Transform child in CardLayoutGroup3.transform)
        {
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);
        }

        foreach(Transform child in DeckList.transform)
        {
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);
        }

        ContinueButton.interactable = false;
        Deck.Clear();
        DisplayList.Clear();
    }
}
