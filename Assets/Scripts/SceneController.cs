using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CardNamespace;

public class SceneController : MonoBehaviour
{
     private float changeTime = 4.5f;

    private int worldID;
    private NodeController activeNode;

    void Start()
    {
        activeNode = MapManager.Instance.nodes[0].sendCurrentNode();
        worldID = activeNode.thisWorld;
    }
   public void BookOneButton()
   {
       SceneManager.LoadScene("Jared");
   }

   public void VictoryFinalBossButton()
   {
        SceneManager.LoadScene("MMTemp");
   }

   public void BookTwoButton()
   {
       SceneManager.LoadScene("Map2");
   }

   public void BookThreeButton()
   {
       SceneManager.LoadScene("MainMenu");
   }



   public void BackToBook()
   {
        if(worldID == 0)
            BookOneButton();
        else if(worldID == 1)
            BookTwoButton();
        else if(worldID == 2)
            BookThreeButton();
   }

}
