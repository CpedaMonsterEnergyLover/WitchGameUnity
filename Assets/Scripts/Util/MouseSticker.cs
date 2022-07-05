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
        Vector3 newPosition = Input.mousePosition;
        newPosition.z = 0;
        transform.position = newPosition;
    }
}
