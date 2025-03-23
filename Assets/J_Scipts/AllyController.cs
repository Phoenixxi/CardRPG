using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyController : MonoBehaviour
{

    public float hoverSpeed = 1.5f;
    public float hoverHeight = 0.5f;
    public float moveSpeed = 3f;
    
    private Vector3 targetPosition;
    private float hoverOffset;

    void Start()
    {
        targetPosition = transform.position;
        hoverOffset = Random.Range(0f, Mathf.PI * 2);
    }
    void Awake()
    {
        DontDestroyOnLoad(gameObject); // Prevents player from being destroyed when loading new scenes
    }


    // Update is called once per frame
    void Update()
    {
        HoverAnimation();
    }
        private void HoverAnimation()
    {
        float hover = Mathf.Sin(Time.time * hoverSpeed + hoverOffset) * hoverHeight;
        transform.position = new Vector3(transform.position.x, targetPosition.y + hover, transform.position.z);
    }

}
