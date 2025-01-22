using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardNamespace;
using System;




public class HandManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform handTransform; // root of hand position
    public float fanSpread = 5f;    // how much hand will spread out
    public List<GameObject> cardsInHand = new List<GameObject>(); // holds cards


    void Start()
    {
        // Starting 5 cards
        AddCardToHand();
        AddCardToHand();
        AddCardToHand();
        AddCardToHand();
        AddCardToHand();
    }

    public void AddCardToHand()
    {
        // Instantiate card
        GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
        // Add new card
        cardsInHand.Add(newCard);

        // Update hand on screen
        UpdateHandVisuals();
    }

    public void UpdateHandVisuals()
    {
        int cardCount = cardsInHand.Count;
        for(int i = 0; i < cardCount; i++)
        {
            float rotationAngle = (fanSpread * (i - (cardCount - 1) / 2f));
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);
        }
    }

}
