using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CardNamespace;

public class SceneController : MonoBehaviour
{
     private float changeTime = 4.5f;
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


     private void Update()
     {
          changeTime -= Time.deltaTime;
          if(changeTime <= 0)
               SceneManager.LoadScene("Combat");
     }


}
