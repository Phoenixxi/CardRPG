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

    //EVENTS -------------------------------------------------------------
    public delegate void CardSelected(RN_Card card);
    public event CardSelected OnCardSelected;

    // Start is called before the first frame update
    void Start()
    {

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
}
