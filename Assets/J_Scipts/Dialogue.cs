using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    private int index = 0;
    private bool isDialogueActive = false;
    public List<GameObject> imageList = new List<GameObject>();

    private int dialogueNum = 2;



    void Start()
    {
        textComponent.text = string.Empty;
        gameObject.SetActive(false);  // Make sure the dialogue UI is hidden initially
        int arraySize = lines.Length;
        if(arraySize == 10)
            dialogueNum = 2;
        else if(arraySize == 4)
            dialogueNum = 4;
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
            if(dialogueNum == 2)
                DisplayCharacterNodeTwo(index);
            else if(dialogueNum == 4)
                DisplayCharacterNodeFour(index);
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            isDialogueActive = false;  // Set flag to false when dialogue finishes
            gameObject.SetActive(false);  // Hide the dialogue UI after last line

        }
    }

    public void DisplayCharacterNodeTwo(int index)
    {
        switch(index)
        {
            case 0:
                imageList[0].SetActive(true);
                break;
            case 1:
                imageList[0].SetActive(true);
                break;
            case 2:
                imageList[0].SetActive(false);
                imageList[1].SetActive(true);
                break;
            case 3:
                imageList[1].SetActive(false);
                imageList[2].SetActive(true);
                break;
            case 4:
                imageList[2].SetActive(false);
                imageList[3].SetActive(true);
                break;
            case 5:
                imageList[3].SetActive(false);
                imageList[2].SetActive(true);
                break;
            case 6:
                imageList[2].SetActive(false);
                imageList[0].SetActive(true);
                break;
            case 7:
                imageList[0].SetActive(false);
                imageList[3].SetActive(true);   
                break;
            case 8:
                imageList[0].SetActive(false);
                imageList[3].SetActive(true);
                break;
            case 9:
                imageList[3].SetActive(false);
                imageList[2].SetActive(true);
                break;
            
        }
    }

        public void DisplayCharacterNodeThreeA(int index)
    {
         switch(index)
        {
            case 0:
                imageList[0].SetActive(true);
                break;
        }
    }

    public void DisplayCharacterNodeFour(int index)
    {
         switch(index)
        {
            case 0:
                imageList[3].SetActive(true);
                break;
            case 1:
                imageList[3].SetActive(false);
                imageList[2].SetActive(true);
                break;
            case 2:
                imageList[2].SetActive(false);
                imageList[1].SetActive(true);
                break;
            case 3:
                imageList[1].SetActive(false);
                imageList[0].SetActive(true);
                break;
        }
    }

        // This method allows other scripts to check if the dialogue is active
    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }

}
