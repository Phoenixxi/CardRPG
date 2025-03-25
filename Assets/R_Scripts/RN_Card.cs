using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using CardNamespace;
using System;

public class RN_Card : MonoBehaviour, IPointerClickHandler
{
    public Card card;
    public TextMeshProUGUI text;
    public int cardCount;

    //EVENTS -------------------------------------------------------------
    public delegate void CardSelected(RN_Card card);
    public event CardSelected OnCardSelected;

    public void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        cardCount = 0;
        text.text = cardCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnCardSelected?.Invoke(this);
    }

    public void Resize()
    {
        UnityEngine.UI.Image image = GetComponent<UnityEngine.UI.Image>();
        image.sprite = card.cardSprite;
        image.SetNativeSize();
    }

    public void addToCount()
    {
        cardCount++;
        text.text = cardCount.ToString();
    }

    public void removeToCount()
    {
        cardCount--;
        text.text = cardCount.ToString();
    }
}
