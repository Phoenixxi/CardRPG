using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public Sprite displaySprite;
    public string[] lines;
    public Sprite[] sprites;
    public float textSpeed;
    private int index = 0;
    private bool isDialogueActive = false;
    public List<GameObject> imageList = new List<GameObject>();




    void Start()
    {
        textComponent.text = string.Empty;
        gameObject.SetActive(false);  // Make sure the dialogue UI is hidden initially
        int arraySize = lines.Length;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            if (textComponent.text == lines[index])
            {
                displaySprite = sprites[index];
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    public void StartDialogue()
    {
        index = 0;
        textComponent.text = string.Empty;  // Clear any previous text
        gameObject.SetActive(true);  // Activate the dialogue UI
        isDialogueActive = true;  // Set flag to true when dialogue starts
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            isDialogueActive = false;  // Set flag to false when dialogue finishes
            gameObject.SetActive(false);  // Hide the dialogue UI after last line

        }
    }



        // This method allows other scripts to check if the dialogue is active
    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }

}
