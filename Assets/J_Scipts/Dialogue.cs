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
    public List<int> IDs = new List<int>();
    [SerializeField]private GameObject DialoguePrefab;
    private GameObject Dialgoue;




    void Start()
    {
        gameObject.SetActive(false);  // Make sure the dialogue UI is hidden initially
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            if (textComponent.text == lines[index])
            {
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
        Dialgoue = Instantiate(DialoguePrefab, transform);
        textComponent = Dialgoue.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        index = 0;
        textComponent.text = string.Empty;  // Clear any previous text
        textComponent.text = lines[index];
        CharacterToDisplay(IDs[index]); // Display the correct character
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
            textComponent.text = string.Empty;
            CharacterToDisplay(IDs[index]);
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

    void CharacterToDisplay(int ID)
    {
        //Remove any character on the screen currently
        foreach(Transform characters in Dialgoue.transform.Find("Characters").transform)
        {
            characters.gameObject.SetActive(false);
        }

        switch (ID)
        {
            case 1:
                Dialgoue.transform.Find("Characters").Find("Mewa").gameObject.SetActive(true);
                break;
            case 2:
                Dialgoue.transform.Find("Characters").Find("Eou").gameObject.SetActive(true);
                break;
            case 3:
                Dialgoue.transform.Find("Characters").Find("BellaBora").gameObject.SetActive(true);
                break;
            case 4:
                Dialgoue.transform.Find("Characters").Find("King").gameObject.SetActive(true);
                break;
            case 5:
                Dialgoue.transform.Find("Characters").Find("Sviur").gameObject.SetActive(true);
                break;
            case 6:
                Dialgoue.transform.Find("Characters").Find("Lune").gameObject.SetActive(true);
                break;
            case 7:
                Dialgoue.transform.Find("Characters").Find("Elio").gameObject.SetActive(true);
                break;
            case 9:
                Dialgoue.transform.Find("Characters").Find("Xue").gameObject.SetActive(true);
                break;
            case 10:
                Dialgoue.transform.Find("Characters").Find("BB").gameObject.SetActive(true);
                break;
            case 11:
                Dialgoue.transform.Find("Characters").Find("BigBoss").gameObject.SetActive(true);
                break;
        }
    }

}
