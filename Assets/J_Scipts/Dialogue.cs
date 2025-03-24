using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // Correct namespace for TextMesh Pro
using UnityEngine.UI;


public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;  // Correct class name
    public string[] lines;
    public float textSpeed;
    private int index;
    private bool isDialogueActive = false;  // Flag to track if dialogue is active

    // public Image characterImage;  // Reference to the Image component for character image
    // public Sprite[] characterSprites;  // Array of sprites for the character images


    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        gameObject.SetActive(false);  // Make sure the dialogue UI is hidden initially
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            if (textComponent.text == lines[index])
                NextLine();
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
        // ShowCharacterImage(0);  // Show the first character image (example)
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
            index++;
            textComponent.text = string.Empty;
            // ShowCharacterImage(index);  // Change character image
            StartCoroutine(TypeLine());
        }
        else
        {
            isDialogueActive = false;  // Set flag to false when dialogue finishes
            gameObject.SetActive(false);  // Hide the dialogue UI after last line
            //             if (characterImage != null)
            // {
            //     characterImage.gameObject.SetActive(false); // Hide the character image after dialogue ends
            // }

        }
    }

        // This method changes the character image based on the current dialogue index
    // void ShowCharacterImage(int characterIndex)
    // {
    //     // Ensure the index is within bounds
    //     if (characterSprites.Length > characterIndex && characterImage != null)
    //     {
    //         characterImage.sprite = characterSprites[characterIndex];  // Set the correct character image
    //         characterImage.gameObject.SetActive(true);  // Make sure the image is visible
    //     }
    // }

        // This method allows other scripts to check if the dialogue is active
    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }

}
