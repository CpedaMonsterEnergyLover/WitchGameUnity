using UnityEngine;

public class InventoryKeyListener : KeyListener
{
    #region Singleton

    public static InventoryKeyListener Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion
    
    public ItemSlot slotUnderCursor;
    
    private void Update()
    {
        if (slotUnderCursor is not null && Input.anyKeyDown)
        {
            slotUnderCursor.OnKeyDown();
        }
    }

    
}
