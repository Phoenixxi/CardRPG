using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class RN_Card : MonoBehaviour, IPointerClickHandler
{
    private TextMeshProUGUI text;
    private RN_DeckScreenManager manager;
    public int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = count + "x";

        manager = GameObject.Find("DeckScreenManager").GetComponent<RN_DeckScreenManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        manager.addToDeck(this);
        text.text = count + "x";
    }
}
