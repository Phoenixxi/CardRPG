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
    private RN_Card selectedCard;
    [SerializeField]private GameObject CardPrefab;

    private void Awake()
    {
        CardLayoutGroup1 = GameObject.Find("CardLayoutGroup1");
        CardLayoutGroup2 = GameObject.Find("CardLayoutGroup2");
        CardLayoutGroup3 = GameObject.Find("CardLayoutGroup3");
        DeckList = GameObject.Find("DeckList");
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
            }
        }
    }

    public void CardClicked(RN_Card card)
    {
        if(Deck.Count >= 20)
        {
            return;
        }

        if(Deck.FindAll(n => n.card == card.card).Count >= 4)
        {
            
        }
        else
        {
            GameObject CCard = Instantiate(CardPrefab);
            RN_Card CCard_script = CCard.GetComponent<RN_Card>();
            CCard_script.card = card.card;
            CCard_script.Resize();
            // Prevent multiple event subscriptions
            CCard_script.OnCardSelected -= CardClicked;
            CCard_script.OnCardSelected += CardClicked;

            CCard.transform.SetParent(DeckList.transform);
            Deck.Add(CCard_script);
        }
    }

    public List<Card> sendDeck(){
        List<Card> actualCards = new List<Card>();
        foreach(RN_Card card in Deck){
            actualCards.Add(card.card);
        }
        return actualCards;
    }
}
