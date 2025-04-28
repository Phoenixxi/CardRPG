using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkedNode : MonoBehaviour
{
    // A list of character cards associated with this forked node
    [SerializeField] private List<CharacterCard> characterCards = new List<CharacterCard>();
    // This method will unlock the character card corresponding to the current node and lock the other one
    public void ManageCharacterCards()
    {
        // World1 Character Selection
        if ((NodeController.activeNode.thisWorld == 0) && (MapManager.Instance != null) && (MapManager.Instance.nodes.Count >= 4))
        {
            //Select King
            if (NodeController.activeNode != null && NodeController.activeNode.ID == 3)
            {
                characterCards[3].unlocked = true;  
                characterCards[2].unlocked = false;
            }
            //Select Bella
            else if (NodeController.activeNode != null && NodeController.activeNode.ID == 4)
            {
                characterCards[2].unlocked = true;  
                characterCards[3].unlocked = false;
            }
            //Select Sviur
            else if (NodeController.activeNode != null && NodeController.activeNode.ID == 6)
            {
                characterCards[4].unlocked = true;
            }
        }

        else if ((NodeController.activeNode.thisWorld == 1) && (MapManager.Instance != null))
        {
            // Debug.Log("World2 being chosen");

            //Select Sviur even if you didn't play through world1.
            if (NodeController.activeNode != null)
            {
                characterCards[4].unlocked = true;
                // Debug.Log("Sviur Unlocked");

            }

            //Select Elio
            if (NodeController.activeNode != null && NodeController.activeNode.ID == 2)
            {
                characterCards[5].unlocked = true;  
                characterCards[6].unlocked = false;
                // Debug.Log("Elio Unlocked");

            }
            //Select Lune
            else if (NodeController.activeNode != null && NodeController.activeNode.ID == 3)
            {
                characterCards[6].unlocked = true;  
                characterCards[5].unlocked = false;
                // Debug.Log("Lune Unlocked");

            }

        }

    }

    // Start is called before the first frame update
    void Start()
    {
        if (characterCards == null)
        {
            characterCards = new List<CharacterCard>();
        }
        if (characterCards.Count < 11) return; // Ensure at elements exist before messing with them
        characterCards[0].unlocked = true;   //Mewa On start
        characterCards[1].unlocked = true;   //Eou  On start
        characterCards[2].unlocked = false;  //Bella Forked
        characterCards[3].unlocked = false;  //King  Forked
        characterCards[4].unlocked = false;  //Sviur Guaranteed after world1 boss battle
        characterCards[5].unlocked = false;  //Elio  Forked
        characterCards[6].unlocked = false;  //Lune  Forked
        characterCards[7].unlocked = false;  //Estella Guarenteed after world1 boss battle
        characterCards[8].unlocked = false;  //Xue   IDK
        characterCards[9].unlocked = false;  //Boris Forked?
        characterCards[10].unlocked = false; //Berta Forked?




    }

    // Update is called once per frame
    // void Update()
    // {
    //     if ((NodeController.activeNode.thisWorld == 1) && (MapManager.Instance != null) && (MapManager.Instance.nodes.Count >= 4))
    //     {
    //         if (NodeController.activeNode == MapManager.Instance.nodes[2] || 
    //             NodeController.activeNode == MapManager.Instance.nodes[3])
    //         {
    //             ManageCharacterCards();
    //         }
    //     }
    // }
}
