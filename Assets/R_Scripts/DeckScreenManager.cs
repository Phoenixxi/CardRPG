using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            SCS = GameObject.Find("SCS");
            SDS = GameObject.Find("SDS");
            SCS.gameObject.SetActive(true);
            SDS.gameObject.SetActive(false);
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
            RN_CharacterScreenManager.DisplayCharacterCards();
        }
    }

    public void CharacterContinue()
    {
        SCS.gameObject.SetActive(false);
        SDS.gameObject.SetActive(true);
        RN_DeckScreenManager.DisplayCards(RN_CharacterScreenManager.characterSelections);
    }

    public void DeckContinue(){
        //TODO: Send to tutorial if two characters, send to combat if more
        SceneManager.LoadScene("Combat");
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
