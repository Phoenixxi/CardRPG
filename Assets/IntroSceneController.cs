using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneController : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable() 
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
