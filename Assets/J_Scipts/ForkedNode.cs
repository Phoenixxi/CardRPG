using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkedNode : MonoBehaviour
{
    // A list of character cards associated with this forked node
    [SerializeField] private List<CharacterCard> characterCards = new List<CharacterCard>();
    // This method will unlock the character card corresponding to the current node and lock the other one
    private void ManageCharacterCards()
    {
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
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (characterCards == null)
        {
            characterCards = new List<CharacterCard>();
        }
        if (characterCards.Count < 5) return; // Ensure at elements exist before messing with them
        characterCards[0].unlocked = true;
        characterCards[1].unlocked = true;
        characterCards[3].unlocked = false;
        characterCards[4].unlocked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (MapManager.Instance != null && MapManager.Instance.nodes.Count >= 4)
        {
            if (NodeController.activeNode == MapManager.Instance.nodes[2] || 
                NodeController.activeNode == MapManager.Instance.nodes[3])
            {
                ManageCharacterCards();
            }
        }
    }
}
