using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardNamespace;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class RN_CharacterScreenManager : MonoBehaviour
{
    public RN_CharacterCard[] characterSelections = {null, null, null};
    private RN_CharacterCard selectedCharacterCard;
    [SerializeField]private List<CharacterCard> characterCards = new List<CharacterCard>();
    [SerializeField]private GameObject CharacterCardPrefab;
    [SerializeField]private UnityEngine.UI.Button continueButton;
    [SerializeField]private UnityEngine.UI.Button removeButton;

    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        continueButton.interactable = false;
        removeButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisplayCharacterCards()
    {
        GameObject CharacterLayoutGroup = GameObject.Find("CharacterLayoutGroup");

        //For each character card, display it by adding it to the grid layout group
        int characterIndexTemp = 0;
        for (int i = 0; i < characterCards.Count(); i++){
            CharacterCard card = characterCards[i];
            //check if we have even unlocked the character
            if(card.unlocked){
                //instantiate a charactercard
                GameObject CCard = Instantiate(CharacterCardPrefab);
                RN_CharacterCard CCard_Script = CCard.GetComponent<RN_CharacterCard>();
                CCard_Script.characterCard = card;
                CCard_Script.Resize();
                CCard_Script.OnCharacterCardSelected += CharacterCardSelected;

                //Add the gameobject to the grid layout
                CCard.transform.SetParent(CharacterLayoutGroup.transform, false);
                CCard.transform.SetSiblingIndex(characterIndexTemp);
                CCard_Script.characterIndex = characterIndexTemp;
                characterIndexTemp++;
            }
        }
    }

    public void DisplayCards()
    {
        GameObject CardLayoutGroup = GameObject.Find("CharacterCardLayoutGroup");
        for(int i = 0; i < CardLayoutGroup.transform.childCount; i++)
        {
            Destroy(CardLayoutGroup.transform.GetChild(i).gameObject);
        }

        foreach(Card card in selectedCharacterCard.characterCard.cards)
        {
            GameObject cardSprite = new GameObject("cardSprite");
            Image image = cardSprite.AddComponent<Image>();
            image.sprite = card.cardSprite;
            cardSprite.transform.SetParent(CardLayoutGroup.transform);
        }
    }

    public void CharacterCardSelected(RN_CharacterCard CCard)
    {
        GameObject CharacterLayoutGroup = GameObject.Find("CharacterLayoutGroup");
        GameObject SelectedCharacterLayoutGroup = GameObject.Find("SelectedCharacterLayoutGroup");

        if(characterSelections.Any(c => c != null && c.characterCard == CCard.characterCard))
        {
            selectedCharacterCard = CCard;
            DisplayCards();
        }
        else
        {
            for(int i = 0; i < characterSelections.Count(); i++){
                if(characterSelections[i] == null){
                    //set CCard as selected
                    characterSelections[i] = CCard;
                    CCard.transform.SetParent(SelectedCharacterLayoutGroup.transform,false);
                    selectedCharacterCard = CCard;

                    // Check if placeholder exists before creating a new one
                    if (CharacterLayoutGroup.transform.childCount > CCard.characterIndex &&
                        CharacterLayoutGroup.transform.GetChild(CCard.characterIndex).name == "MyEmptyObject")
                    {
                        return;
                    }

                    //Create a placeholder in the grid layout
                    GameObject emptyObject = new GameObject("MyEmptpyObject");
                    RectTransform referenceRect = CCard.GetComponent<RectTransform>();
                    RectTransform targetRect = emptyObject.AddComponent<RectTransform>();
                    targetRect.sizeDelta = referenceRect.sizeDelta;
                    emptyObject.transform.SetParent(CharacterLayoutGroup.transform);
                    emptyObject.transform.SetSiblingIndex(CCard.characterIndex);

                    //Because we have at least one character selected, make continue button interactable
                    continueButton.interactable = true;

                    //Display the selected character cards
                    DisplayCards();
                    break;
                }
            }
        }

        if(selectedCharacterCard == null){
            removeButton.interactable = false;
        }else{
            removeButton.interactable = true;
        }
    }

    public void RemoveCharacter()
    {
        GameObject CharacterLayoutGroup = GameObject.Find("CharacterLayoutGroup");
        GameObject CardLayoutGroup = GameObject.Find("CharacterCardLayoutGroup");

        if(selectedCharacterCard != null)
        {
            Debug.Log(selectedCharacterCard.characterIndex);
            //Remove the placeholder from the grid layout
            Destroy(CharacterLayoutGroup.transform.GetChild(selectedCharacterCard.characterIndex).gameObject);

            //Return the selectedcharacter back to the character display side
            selectedCharacterCard.transform.SetParent(CharacterLayoutGroup.transform);
            selectedCharacterCard.transform.SetSiblingIndex(selectedCharacterCard.characterIndex);

            //Reset the characterselections array
            for(int i = 0; i < characterSelections.Count(); i++){
                if(characterSelections[i] == selectedCharacterCard){
                    characterSelections[i] = null;
                    break;
                }
            }

            //Stop displaying the removed character's cards
            for(int i = 0; i < CardLayoutGroup.transform.childCount; i++)
            {
                Destroy(CardLayoutGroup.transform.GetChild(i).gameObject);
            }
            selectedCharacterCard = null;

            continueButton.interactable=false;
            for(int i = 0; i < characterSelections.Count(); i++){
                if(characterSelections[i] != null){
                    continueButton.interactable = true;
                }
            }
        }

        if(selectedCharacterCard == null){
            removeButton.interactable = false;
        }else{
            removeButton.interactable = true;
        }
    }

    public int[] sendCharacterID()
    {
        int[] IDs = new int[3];

        for(int i = 0; i < characterSelections.Count(); i++){
            IDs[i] = characterSelections[i].characterCard.ID;
        }

        return IDs;
    }
}
