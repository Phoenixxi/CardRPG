using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class RN_DeckScreenManager : MonoBehaviour
{
    //array to hold which character slot has been selected or not
    private RN_CharacterCard[] characterSelections = {null, null, null};
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clickedCharacter(RN_CharacterCard characterCard)
    {
        for(int i = 0; i < characterSelections.Length; i++){
            if(characterSelections[i] == null){
                characterSelections[i] = characterCard;
            }
        }
    }
}
