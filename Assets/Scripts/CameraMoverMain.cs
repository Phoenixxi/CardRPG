using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoverMain : MonoBehaviour
{
    // Assigned in inspector
   public Transform targetTransform;
    public float moveSpeed = 3f;
     public float rotateSpeed = 10f;

    private bool shouldMove = false;

    private void Update()
    {
        if (shouldMove)
        {
            // Move towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, moveSpeed * Time.deltaTime);
            Quaternion targetRotation = targetTransform.rotation;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,  rotateSpeed * Time.deltaTime * 5 );

            // Stop when both position and rotation are close enough
            if (Vector3.Distance(transform.position, targetTransform.position) < 0.01f &&
                Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                shouldMove = false;
            }
        }
    }

    public void MoveCamera()
    {
        shouldMove = true;
    }
}
