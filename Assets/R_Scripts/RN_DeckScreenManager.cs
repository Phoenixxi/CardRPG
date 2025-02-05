using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class RN_DeckScreenManager : MonoBehaviour
{
    //array to hold which character slot has been selected or not
    private RN_CharacterCard[] characterSelections = {null, null, null};
    private Vector3 characterSlot1;
    private Vector3 characterSlot2;
    private Vector3 characterSlot3;
    // Start is called before the first frame update
    void Start()
    {
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
                break;
            }
        }
    }

    public void removeCharacter(RN_CharacterCard characterCard){
        for(int i = 0; i < characterSelections.Length; i++){
            if(characterSelections[i] != null && characterSelections[i].Equals(characterCard)){
                characterCard.rectTransform.anchoredPosition = characterCard.originalPos;
                characterCard.choosen = false;
                characterSelections[i] = null;
                break;
            }
        }
    }
}
