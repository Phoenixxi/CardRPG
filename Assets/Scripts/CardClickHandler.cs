using System.Collections;
using System.Collections.Generic;
using System;
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

    [SerializeField] private GameObject highlightEffect;
    [SerializeField] private GameObject reshuffleButton;
    [SerializeField] private GameObject costManipButton;


    public Vector3 GetOGPosition(){
        return OGposition;
    }

    public Vector3 GetOGSelectedPosition(){
        return OGSelectedPosition;
    }
    
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
            OGposition = transform.localPosition;
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
        int energyPool = handManager.energyPool;
        
        // card energy cost
        int energyCost = GetComponent<CardDisplay>().cardData.Energy;

        int subFromPool = energyCost - energy;

        if(EnemyTurn || handManager.blackOverlay.gameObject.activeSelf 
            || GetComponent<CardDisplay>().cardData.Reshuffle 
            || GetComponent<CardDisplay>().cardData.CostManipulation)
            return;

        // PREVIOUSLY NOT SELECTED - CLICK TO SELECT
        if (!isSelected && (energy - energyCost) >= 0)
        {
            transform.localPosition = selectedPosition;
            OGSelectedPosition = selectedPosition;
            isSelected = !isSelected;
        }   
        // PREVIOUSLY NOT SELECTED - NOT ENOUGH MONEY
        else if (!isSelected && (energy - energyCost) < 0)
        {   
            // Check energy pool
            if(!isSelected && ((energy + energyPool) - energyCost) >= 0)
            {
                handManager.energyPool -= subFromPool;
                handManager.energyPoolText.text = handManager.energyPool.ToString();

                handManager.currentEnergy = 0;
                handManager.energyText.text = "0";

                transform.localPosition = selectedPosition;
                OGSelectedPosition = selectedPosition;
                isSelected = !isSelected;
            }
            return;
        }
        // PREVIOUSLY SELECTED - CLICK TO DISSELECT
        else
        {
            transform.localPosition = originalPosition;
            isSelected = !isSelected;

            // FIX THIS put small energy circle back
                int spaceInSmall = (3 - energyPool);
                int energyToSmall = Math.Min(energyCost, spaceInSmall);;
                int energyToBig = energyCost - energyToSmall;
                
                // Set new energy in display and code
                handManager.energyPool += energyToSmall;
                handManager.energyPoolText.text = handManager.energyPool.ToString();
                handManager.currentEnergy += energyToBig;
                handManager.energyText.text = handManager.currentEnergy.ToString();
                
                return;
            
        }
        // update energy in hand manger
        handManager.UpdateEnergy(energyCost, isSelected);
    }

    public void SetHandManager(HandManager manager)
    {
        handManager = manager;
    }

    public void ReshuffleButtonPressed()
    {
        if(handManager.getCostJustChanged())
            return;
        int energy = handManager.currentEnergy;
        if(energy >= 2){
            // decreases by 2????
            handManager.currentEnergy -= 1;
            handManager.energyText.text = handManager.currentEnergy.ToString();
            handManager.ReshuffleCards();
        }
    }

    public void CostManipulationButtonPressed()
    {
        int energy = handManager.currentEnergy;
        if(energy >= 3){
            handManager.currentEnergy -= 3;
            handManager.energyText.text = handManager.currentEnergy.ToString();
            handManager.CostManipulationDisplayUpdate();
            
        }
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(EnemyTurn || handManager.blackOverlay.gameObject.activeSelf)
            return;

        highlightEffect.SetActive(true);

        // If this is reshuffle card, show button
        if(GetComponent<CardDisplay>().cardData.Reshuffle)
            reshuffleButton.SetActive(true);

        // If cost manipulation card, show button
        if(GetComponent<CardDisplay>().cardData.CostManipulation)
            costManipButton.SetActive(true);

        transform.localScale = originalScale * selectScale; 

        if(handManager != null)
            handManager.AdjustHandForHoveredCard(this);

    }      

    public void OnPointerExit(PointerEventData eventData)
    {
        if(EnemyTurn || handManager.blackOverlay.gameObject.activeSelf)
            return;
        // Deactivate the CardHighlight image
        highlightEffect.SetActive(false);

        if(GetComponent<CardDisplay>().cardData.Reshuffle)
            reshuffleButton.SetActive(false);

        if(GetComponent<CardDisplay>().cardData.CostManipulation)
            costManipButton.SetActive(false);
        

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
