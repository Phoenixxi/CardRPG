using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardNamespace;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    private int teamHealth;
    
    public OptionsManager OptionsManager {get; private set;}
    //public AudioManager AudioManager {get; private set;}
    //public DeckManager DeckManager {get; private set;}
    public VictoryLossManager VictoryLossManager {get; private set;}

    private void Awake()
    {
        // Transfer data between scenes
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeManagers();
        }
        else if(Instance != null)
        {
            Destroy(gameObject);
        }
    }

    private void InitializeManagers()
    {
        OptionsManager = GetComponentInChildren<OptionsManager>();
        //AudioManager = GetComponentInChildren<AudioManager>();
       // DeckManager = GetComponentInChildren<DeckManager>();
        VictoryLossManager = GetComponentInChildren<VictoryLossManager>();


        if(OptionsManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/OptionsManager");
            if(prefab == null)
            {
                Debug.Log($"OptionsManager prefab not found");
            }
            else
            {
                Instantiate(prefab, transform.position, Quaternion.identity, transform);
                OptionsManager = GetComponentInChildren<OptionsManager>();
            }
        }

        

        
            
    }

    public int TeamHealth
    {
        get { return teamHealth;}
        set {teamHealth = value;}
    }
}
