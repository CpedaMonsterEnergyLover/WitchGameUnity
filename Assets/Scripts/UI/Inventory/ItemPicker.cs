using UnityEngine;

public class ItemPicker : MonoBehaviour
{
    void Update()
    {
        UpdatePosition();
    }

    private void OnEnable()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 5;
        mousePosition.x -= 34;
        mousePosition.y += 34;
        transform.position = mousePosition;
    }
}
