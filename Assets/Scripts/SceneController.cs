using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CardNamespace;

public class SceneController : MonoBehaviour
{
     private float changeTime = 4.5f;

   public void BookOneButton()
   {
       SceneManager.LoadScene("Jared");
   }

   public void BookTwoButton()
   {
       SceneManager.LoadScene("Map2");
   }

   public void BookThreeButton()
   {
       SceneManager.LoadScene("Map3");
   }

}
