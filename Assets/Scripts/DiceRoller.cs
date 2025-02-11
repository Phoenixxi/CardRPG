using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoller : MonoBehaviour
{
    // temporary text display for the prototype
   public Text resultText; 
   // energy text field
    public Text energyText; 
    public int diceResult = 0;

    private void Start()
    {
        resultText.text = "";
        energyText.text = "Energy: 0";
    }

    public void RollDice()
    {
        // Random number between 1-10
        diceResult = Random.Range(1, 11); 
        StartCoroutine(ShowResult(diceResult));
    }

    private IEnumerator ShowResult(int diceResult)
    {
        // temporary for prototype before we get roll animation
        resultText.text = "Rolling...";
        yield return new WaitForSeconds(1.5f);
        resultText.text = diceResult.ToString();
        energyText.text = "Energy: " + diceResult;

        // Clear temporary result and update Energy text
        yield return new WaitForSeconds(1f);
        resultText.text = "";
    }
}
