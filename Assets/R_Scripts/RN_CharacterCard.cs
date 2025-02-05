using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System;

public class RN_CharacterCard : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IEquatable<RN_CharacterCard>
{
    //Deck screen manager object
    private RN_DeckScreenManager manager;
    public RectTransform rectTransform;
    public Vector3 originalPos;
    public string characterName;
    public bool choosen;
    // Start is called before the first frame update
    void Start()
    {
        //set the size of the image to be its native size
        GetComponent<Image>().SetNativeSize();

        choosen = false;

        //Find the deck screen manager
        manager = GameObject.Find("DeckScreenManager").GetComponent<RN_DeckScreenManager>();
        rectTransform = GetComponent<RectTransform>();
        originalPos = rectTransform.anchoredPosition;
        characterName = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //method to check if the mouse is hovering the card
    public void OnPointerEnter(PointerEventData eventData){
        Debug.Log("hi");
    }

    //method to check if the mouse clicked on the card
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!choosen){
            manager.selectCharacter(this);
        }else{
            manager.removeCharacter(this);
        }
        /*
        Vector2 size = rectTransform.sizeDelta;
        rectTransform.sizeDelta = new Vector2(size.x * 2, size.y * 2);
        */
    }

    public bool Equals(RN_CharacterCard other)
    {
        if(other == null){
            return false;
        }

        return other.characterName == characterName;
    }
}
