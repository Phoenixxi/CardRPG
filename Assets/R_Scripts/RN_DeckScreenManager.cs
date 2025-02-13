using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RN_DeckScreenManager : MonoBehaviour
{
    //array to hold which character slot has been selected or not
    private RN_CharacterCard[] characterSelections = {null, null, null};
    private int characterSelected = 0;
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



    //Variables for DeskSelectionScreen
    [SerializeField]
    private GameObject cardPrefab;
    private GameObject SelectDeckScene;
    private GameObject deckList;


    [SerializeField]
    private string sceneToLoad;


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

        SelectDeckScene = GameObject.Find("SelectDeckScreen");
        deckList = GameObject.Find("DeckList");
        SelectDeckScene.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// BELOW ARE ALL CODE FOR THE SELECTCHARACTERSCENE
    /// </summary>
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
                characterSelected++;
                continueButton.interactable = true;
                displayCharacterCards(characterCard);
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
            characterSelected--;

            if(characterSelected == 0){
                continueButton.interactable = false;
            }
        }
    }

    public void displayCharacterCards(RN_CharacterCard characterCard){
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

    public void continueCharacterButtonClicked(){
        GameObject SelectCharacterScene = GameObject.Find("SelectCharacterScreen");
        //SelectCharacterScene.transform.position = new Vector2(9999,9999);
        SelectCharacterScene.SetActive(false);
        SelectDeckScene.SetActive(true);
        displayCards();
    }






    /// <summary>
    /// BELOW ARE ALL CODE FOR THE SELECTDECKSCENE
    /// </summary>
    public void displayCards(){
        GameObject firstRow = GameObject.Find("FirstRow");
        GameObject secondRow = GameObject.Find("SecondRow");
        GameObject thirdRow = GameObject.Find("ThirdRow");
        int row = 1;

        for(int i = 0; i < characterSelections.Length; i++){
            if(characterSelections[i] == null){
                continue;
            }
            for(int j = 0; j < characterSelections[i].cardSprites.Count; j++){
                GameObject card = Instantiate(cardPrefab);
                UnityEngine.UI.Image image;
                card.TryGetComponent<UnityEngine.UI.Image>(out image);
                image.sprite = characterSelections[i].cardSprites[j];
                image.SetNativeSize();
                switch (row)
                {
                    case 1:
                        card.transform.SetParent(firstRow.transform);
                        break;
                    case 2:
                        card.transform.SetParent(secondRow.transform);
                        break;
                    case 3:
                        card.transform.SetParent(thirdRow.transform);
                        break;
                }
            }
            row++;
        }
    }

    public void addToDeck(RN_Card card){
        GameObject imageGO = new GameObject("image");
        UnityEngine.UI.Image image = imageGO.AddComponent<UnityEngine.UI.Image>();
        UnityEngine.UI.Image imageCopy;
        card.TryGetComponent<UnityEngine.UI.Image>(out imageCopy);
        image.sprite = imageCopy.sprite;
        image.SetNativeSize();
        imageGO.transform.SetParent(deckList.transform);
        card.count++;
    }

    public void continueDeckButton(){
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("No scene assigned for this node.");
        }
    }
}
