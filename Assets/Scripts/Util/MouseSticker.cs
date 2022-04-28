using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSticker : MonoBehaviour
{
    public Camera playerCamera;
    
    private void Update()
    {
        UpdatePosition();
    }

    private void OnEnable()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector3 newPosition = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0;
        transform.position = newPosition;
    }
}
