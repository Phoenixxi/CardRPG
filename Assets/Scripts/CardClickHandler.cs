using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using CardNamespace;
using UnityEngine.UI;

public class CardClickHandler : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    private Canvas canvas;
    private bool isSelected = false;
     public float moveUpOffset = 30f;
     private float selectScale = 1.1f;

    // Original positions
     private Vector3 originalScale;
    private Vector3 originalPosition;
    private Vector3 selectedPosition;
    private HandManager handManager;

    [SerializeField] private GameObject highlightEffect;
    

        private void Start()
        {
            // Save the original position of the card
            originalPosition = transform.localPosition;
            originalScale = transform.localScale;
            // Calculate the selected position
            selectedPosition = originalPosition + new Vector3(0, moveUpOffset, 0);
            //highlightEffect.SetActive(true);

        }

        void Awake()
        {
            canvas = GetComponent<Canvas>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // current energy to spend
            int energy = handManager.currentEnergy;
            
            // card energy cost
             int energyCost = GetComponent<CardDisplay>().cardData.Energy;

            // PREVIOUSLY NOT SELECTED - CLICK TO SELECT
            if (!isSelected && (energy - energyCost) >= 0)
            {
                 //int energyCost = GetComponent<CardDisplay>().cardData.Energy;
                 transform.localPosition = selectedPosition;
                 isSelected = !isSelected;
                
            }   
            // PREVIOUSLY SELECTED - CLICK TO DISSELECT
            else if (!isSelected && (energy - energyCost) < 0)
            {
                return;
            }
            else
            {
                transform.localPosition = originalPosition;
                isSelected = !isSelected;
            }
                // update energy in hand manger
                handManager.UpdateEnergy(energyCost, isSelected);
        }

        public void SetHandManager(HandManager manager)
        {
            handManager = manager;  // Set the reference to HandManager
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            highlightEffect.SetActive(true);
            transform.localScale = originalScale * selectScale; 
        }      

        public void OnPointerExit(PointerEventData eventData)
        {
            // Deactivate the CardHighlight image
            highlightEffect.SetActive(false);

            // Reset the scale of the card
            transform.localScale = originalScale;
        }      
}
