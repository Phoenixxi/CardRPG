using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardNamespace;
using System;



public class HandManager : MonoBehaviour
{
    public DeckManager deckManager;
    public GameObject cardPrefab;
    public Transform handTransform; // root of hand position
    public float fanSpread = -7.5f;    // angle of cards
    public float cardSpacing = 160f;  // how much hand will spread out
    public float verticalSpacing = 53f; // pull cards on sides down
    public int maxHandSize = 5;
    public List<GameObject> cardsInHand = new List<GameObject>(); // holds cards


    void Start()
    {
        // Starting 5 cards


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
        }
        // Update hand on screen
        UpdateHandVisuals();
    }

    void Update()
    {
        // UpdateHandVisuals();
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
    }

}
