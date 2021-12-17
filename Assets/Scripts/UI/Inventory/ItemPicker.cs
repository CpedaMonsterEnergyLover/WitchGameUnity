using UnityEngine;

public class ItemPicker : MonoBehaviour
{
    #region Singleton

    public static ItemPicker Instance;

    private void Awake()
    {
        if (Instance is null) Instance = this;
        else Debug.LogError("Found multiple instances of itempicker");
        gameObject.SetActive(false);
    }

    #endregion

    public InventorySlot itemSlot;

    public void SetItem(Item item, int count)
    {
        gameObject.SetActive(true);
        itemSlot.AddItem(item, count);
        Tooltip.Instance.SetEnabled(false);
    }

    public void Clear()
    {
        gameObject.SetActive(false);
        itemSlot.Clear();
        Tooltip.Instance.SetEnabled(true);
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
    
    private void Update()
    {
        UpdatePosition();
    }

}
