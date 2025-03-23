using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System;

public class RN_CharacterCard : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IEquatable<RN_CharacterCard>
{
    public CharacterCard characterCard;
    public int characterIndex;

    //EVENTS -------------------------------------------------------------
    public delegate void CharacterCardSelected(RN_CharacterCard characterCard);
    public event CharacterCardSelected OnCharacterCardSelected;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resize(){
        UnityEngine.UI.Image image = GetComponent<UnityEngine.UI.Image>();
        image.sprite = characterCard.sprite;
        image.SetNativeSize();
    }
    
    //method to check if the mouse is hovering the card
    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    //method to check if the mouse clicked on the card
    public void OnPointerClick(PointerEventData eventData)
    {
        OnCharacterCardSelected?.Invoke(this);
    }

    public bool Equals(RN_CharacterCard other)
    {
        if(other.characterCard == null) return false;
        return other.characterCard == characterCard;
    }
}
