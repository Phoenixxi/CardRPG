using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeckScreenManager : MonoBehaviour
{
    public static DeckScreenManager Instance {get; private set;}
    private string originalScene;

    public RN_CharacterScreenManager RN_CharacterScreenManager { get; private set;}
    public RN_DeckScreenManager RN_DeckScreenManager { get; private set;}

    private GameObject SCS;
    private GameObject SDS;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            if(originalScene == null)
            {
                originalScene = SceneManager.GetActiveScene().name;
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            DontDestroyOnLoad(gameObject);
            InitializeManagers();
        }
    }

    private void InitializeManagers(){
        RN_CharacterScreenManager = GetComponentInChildren<RN_CharacterScreenManager>();
        RN_DeckScreenManager = GetComponentInChildren<RN_DeckScreenManager>();
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        if(scene.name == originalScene){
            StartCoroutine(InitializeSceneObjects());
            RN_CharacterScreenManager.reset();
            SCS.gameObject.SetActive(true);
            SDS.gameObject.SetActive(false);

            //start displaying the character cards
            RN_CharacterScreenManager.DisplayCharacterCards();
        }
    }

    private IEnumerator InitializeSceneObjects()
    {
        SCS = GameObject.Find("SCS");
        SDS = GameObject.Find("SDS");

        //Set the buttons
        RN_CharacterScreenManager.continueButton = SCS.gameObject.transform.Find("Buttons/ContinueButton")?.GetComponent<Button>();
        RN_CharacterScreenManager.continueButton.onClick.AddListener(CharacterContinue);
        RN_CharacterScreenManager.removeButton = SCS.gameObject.transform.Find("Buttons/RemoveButton")?.GetComponent<Button>();
        RN_CharacterScreenManager.removeButton.onClick.AddListener(RN_CharacterScreenManager.RemoveCharacter);

        RN_DeckScreenManager.ContinueButton = SDS.gameObject.transform.Find("Buttons/ContinueButton")?.GetComponent<Button>();
        RN_DeckScreenManager.ContinueButton.onClick.AddListener(DeckContinue);
        RN_DeckScreenManager.BackButton = SDS.gameObject.transform.Find("Buttons/BackButton")?.GetComponent<Button>();
        RN_DeckScreenManager.BackButton.onClick.AddListener(DeckBack);

        RN_DeckScreenManager.Initialize();
        yield return new WaitForSeconds(1);
    }

    public void CharacterContinue()
    {
        RN_DeckScreenManager.reset();
        SCS.gameObject.SetActive(false);
        SDS.gameObject.SetActive(true);
        RN_DeckScreenManager.DisplayCards(RN_CharacterScreenManager.characterSelections);
    }

    public void DeckContinue(){
        //TODO: Send to tutorial if two characters, send to combat if more
        SceneManager.LoadScene("Combat");
    }

    public void DeckBack(){
        RN_DeckScreenManager.reset();
        SCS.gameObject.SetActive(true);
        SDS.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
