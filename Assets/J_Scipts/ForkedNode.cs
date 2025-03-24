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
        characterCards[0].unlocked = true;
        characterCards[1].unlocked = true;

        if (MapManager.Instance != null && MapManager.Instance.nodes.Count >= 4 && characterCards.Count >= 4){

            if ((NodeController.thisNode.ID == 3) == MapManager.Instance.nodes[2])
            {
                characterCards[2].unlocked = true;  
                characterCards[3].unlocked = false;
            }
            else if (NodeController.activeNode == MapManager.Instance.nodes[3])
            {
                characterCards[3].unlocked = true;  
                characterCards[2].unlocked = false;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // You can add logic here if you need to check or update based on conditions
    if (NodeController.activeNode == MapManager.Instance.nodes[2] || NodeController.activeNode == MapManager.Instance.nodes[3])
        ManageCharacterCards();
    }
}
