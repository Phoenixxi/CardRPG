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
            // Toggle the selection state
            isSelected = !isSelected;

            // Move the card based on the selection state
            if (isSelected)
            {
                transform.localPosition = selectedPosition;
            }
            else
            {
                transform.localPosition = originalPosition;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("OnPointerEnter called: Mouse entered the card.");
            highlightEffect.SetActive(true);
            transform.localScale = originalScale * selectScale; 
        }      

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("OnPointerExit called");
            // Deactivate the CardHighlight image
            highlightEffect.SetActive(false);

            // Reset the scale of the card
            transform.localScale = originalScale;
        }      
}
