using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
   public void PlayGame ()
   {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
   }


   public void QuitGame()
   {
        Debug.Log("QUIT!");
        Application.Quit();
   }


     public void ReturnGame()
     {
          SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
     }


     public void ReturnMenu()
     {
          SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
     }


}
