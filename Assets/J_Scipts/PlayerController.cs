using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float hoverSpeed = 1.5f;
    public float hoverHeight = 0.2f;
    public float moveSpeed = 5f;
    
    private Vector3 targetPosition;
    private bool isMoving = false;
    private float hoverOffset;

    void Start()
    {
        targetPosition = transform.position;
        hoverOffset = Random.Range(0f, Mathf.PI * 2);
    }

    void Update()
    {
        HoverAnimation();

        if (isMoving)
        {
            MoveToTarget();
        }
    }

    private void HoverAnimation()
    {
        float hover = Mathf.Sin(Time.time * hoverSpeed + hoverOffset) * hoverHeight;
        transform.position = new Vector3(transform.position.x, targetPosition.y + hover, transform.position.z);
    }

    private void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
        {
            transform.position = targetPosition;
            isMoving = false;
            //Debug.Log("Reached node: " + targetPosition);

        }
    }

    public void MoveToNode(Vector3 nodePosition)
    {
        //Debug.Log("Node Clicked: " + gameObject.name);
        float heightOffset = 0.5f; // Adjust this to fit your game
        targetPosition = new Vector3(nodePosition.x, nodePosition.y + heightOffset, nodePosition.z);
        isMoving = true;
    }
}
