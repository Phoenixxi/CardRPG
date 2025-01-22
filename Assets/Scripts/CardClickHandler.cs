using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardClickHandler : MonoBehaviour, IPointerClickHandler
{
    private bool isSelected = false;
    private Vector3 originalPosition;
    private Vector3 selectedPosition;

    // Offset to move the card up when selected
    public float moveUpOffset = 30f;

    private void Start()
    {
        // Save the original position of the card
        originalPosition = transform.localPosition;
        // Calculate the selected position
        selectedPosition = originalPosition + new Vector3(0, moveUpOffset, 0);
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
}
