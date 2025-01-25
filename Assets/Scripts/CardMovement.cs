/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
   private RectTransform rectTransform;
   private Canvas canvas;
   
   // Mouse Pointer Position
   private Vector2 originalLocalPointerPosition;
   private Vector3 originalPanelLocalPosition;
   private Vector3 originalScale;
   private int currentState = 0;
   private Quaternion originalRotation;
   private Vector3 originalPosition;

   [SerializeField] private float selectScale = 1.1f;
   [SerializeField] private Vector2 cardPlay;
   [SerializeField] private Vector3 playPosition;
   [SerializeField] private GameObject highlightEffect;
   [SerializeField] private GameObject playArrow;

   void Awake()
   {
      rectTransform = GetComponent<rectTransform>();
      canvas = GetComponent<Canvas>();
      originalScale = rectTransform.localScale;
      originalPosition = rectTransform.localPosition;
      originalRotation = rectTransform.localRotation;
   }

   void Update()
   {
      switch(currentState)
      {
         case 1:
            HandleHoverState();
            break;
         case 2:
            HandleDragState();
            if(!Input.GetMouseButton(0))  // Check if mouse button is released
               TransitionToState0();
            break;
         case 3:
            HandlePlayState();
            if(!Input.GetMouseButton(0))  // Check if mouse button is released
               TransitionToState0();
            break;
      }
   }

   private void TransitionToState0()
   {
      currentState = 0;
      
   }

   private void OnPointerEnter(PointerEventData eventData)
   {
      if(currentState == 0)
      {
         originalPosition = rectTransform.localPosition;
         originalRotation = rectTransform.localRotation;
         originalScale = rectTransform.localScale;

         currentState = 1;
      }
   }

   public void OnPointerExit(PointerEventData eventData)
   {
      if(currentState == 1)
      {
         currentState = 0;
         TransitionToState0();
      }
   }

   public void OnPointerDown(PointerEventData eventData)
   {
      if(currentState == 1)
      {
         currentState = 2;
         RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out originalLocalPointerPosition);
         originalPanelLocalPosition = rectTransform.localPosition;
      }
   }

   private void HandleHoverState()
   {
      highlightEffect.SetActive(true);
      rectTransform.localScale = originalScale * selectScale; 
   }
}

*/