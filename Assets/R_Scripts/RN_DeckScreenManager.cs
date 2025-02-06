using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RN_DeckScreenManager : MonoBehaviour
{
    //array to hold which character slot has been selected or not
    private RN_CharacterCard[] characterSelections = {null, null, null};
    [SerializeField]
    private List<UnityEngine.UI.Image> characterCardDisplay;
    [SerializeField]
    private List<UnityEngine.UI.Button> buttons;
    [SerializeField]
    private UnityEngine.UI.Button continueButton;
    private RN_CharacterCard displaying;
    private Canvas canvas;
    private Vector3 characterSlot1;
    private Vector3 characterSlot2;
    private Vector3 characterSlot3;
    // Start is called before the first frame update
    void Start()
    {
        canvas = FindAnyObjectByType<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("No Canvas found in the scene.");
        }

        for(int i = 0; i < characterCardDisplay.Count; i++){
            characterCardDisplay[i].gameObject.SetActive(false);
        }

        for(int i = 0; i < buttons.Count; i++){
            buttons[i].gameObject.SetActive(false);
            var i1 = i;
            buttons[i].onClick.AddListener(delegate{removeCharacter(i1);});
        }

        continueButton.interactable = false;

        characterSlot1 = new Vector3(-800, 342, 0);
        characterSlot2 = new Vector3(-500, 342, 0);
        characterSlot3 = new Vector3(-200, 342, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectCharacter(RN_CharacterCard characterCard)
    {
        if(characterCard.rectTransform == null){
            Debug.LogError("rectTransform is null for characterCard: " + characterCard.name);
        }
        
        for(int i = 0; i < characterSelections.Length; i++){
            if(characterSelections[i] == null){
                characterSelections[i] = characterCard;
                characterCard.choosen = true;
                switch (i){
                    case 0:
                        characterCard.rectTransform.anchoredPosition = characterSlot1;
                        break;
                    case 1:
                        characterCard.rectTransform.anchoredPosition = characterSlot2;
                        break;
                    case 2:
                        characterCard.rectTransform.anchoredPosition = characterSlot3;
                        break;
                }
                buttons[i].gameObject.SetActive(true);
                displayCards(characterCard);
                break;
            }
        }
    }

    public void removeCharacter(int index){
        if(characterSelections[index] != null){
            characterSelections[index].rectTransform.anchoredPosition = characterSelections[index].originalPos;
            characterSelections[index].choosen = false;
            if(displaying != null && characterSelections[index].Equals(displaying)){
                for(int i = 0; i < characterCardDisplay.Count; i++){
                    characterCardDisplay[i].gameObject.SetActive(false);
                }
            }
            characterSelections[index] = null;
            buttons[index].gameObject.SetActive(false);
        }
    }

    public void displayCards(RN_CharacterCard characterCard){
        for(int i = 0; i < characterCard.cardSprites.Count; i++){
            if(characterCardDisplay[i] == null || characterCard.cardSprites[i] == null){
                Debug.LogError($"Missing sprite or display element at index {i}");
                continue;
            }
            characterCardDisplay[i].sprite = characterCard.cardSprites[i];
            characterCardDisplay[i].SetNativeSize();
            Vector2 size = characterCardDisplay[i].rectTransform.sizeDelta;
            characterCardDisplay[i].rectTransform.sizeDelta = new Vector2(size.x / 1.5f, size.y / 1.5f);
            characterCardDisplay[i].gameObject.SetActive(true);
        }
        displaying = characterCard;
    }
}
