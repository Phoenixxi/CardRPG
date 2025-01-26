using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    private int teamHealth;

    private void Awake()
    {
        // Transfer data between scenes
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(Instance != null)
        {
            Destroy(gameObject);
        }
    }

    public int TeamHealth
    {
        get { return teamHealth;}
        set {teamHealth = value;}
    }
}
