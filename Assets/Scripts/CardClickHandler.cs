using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
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

     public bool EnemyTurn = false;

    // Original positions
     private Vector3 originalScale;
    private Vector3 originalPosition;
    public Vector3 OGposition;
    public Vector3 OGSelectedPosition;
    private List<Vector3> originalPositions = new List<Vector3>();
    private Vector3 selectedPosition;
    private HandManager handManager;

    public Vector3 GetOGPosition(){
        return OGposition;
    }

    public Vector3 GetOGSelectedPosition(){
        return OGSelectedPosition;
    }

    [SerializeField] private GameObject highlightEffect;
    
        public void ToggleEnemyTurn(bool state)
        {
            EnemyTurn = state;
        }

        private void Start()
        {
            // Save the original position of the card
            originalPosition = transform.localPosition;
            OGposition = transform.localPosition;
            originalScale = transform.localScale;
            originalPositions.Add(originalPosition);
            // Calculate the selected position
            selectedPosition = originalPosition + new Vector3(0, moveUpOffset, 0);
            //highlightEffect.SetActive(true);
        }

        public void UpdateCardPositions()
        {
            if(!isSelected){
                originalPosition = transform.localPosition;
                originalScale = transform.localScale;
                selectedPosition = originalPosition + new Vector3(0, moveUpOffset, 0);
            }
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

             if(EnemyTurn)
                return;

            // PREVIOUSLY NOT SELECTED - CLICK TO SELECT
            if (!isSelected && (energy - energyCost) >= 0)
            {
                 //int energyCost = GetComponent<CardDisplay>().cardData.Energy;
                 transform.localPosition = selectedPosition;
                 OGSelectedPosition = selectedPosition;
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
            handManager = manager;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(EnemyTurn)
                return;

            highlightEffect.SetActive(true);
            transform.localScale = originalScale * selectScale; 

            if(handManager != null)
                handManager.AdjustHandForHoveredCard(this);

        }      

        public void OnPointerExit(PointerEventData eventData)
        {
            if(EnemyTurn)
                return;
            // Deactivate the CardHighlight image
            highlightEffect.SetActive(false);

            // Reset the scale of the card
            transform.localScale = originalScale;

             if(handManager != null)
                handManager.ResetHandPositions(this);
            
        }    
    public bool IsSelected()
    {
        return isSelected;
    }  
}
