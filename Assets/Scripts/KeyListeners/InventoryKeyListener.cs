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
    
    public ItemSlot SlotUnderCursor { get; private set; }

    private void SetSlotUnderCursor(ItemSlot slot) => SlotUnderCursor = slot;
    private void ResetSlotUnderCursor() => SlotUnderCursor = null;

    private void OnInventoryClosed(WindowIdentifier windowIdentifier)
    {
        if(windowIdentifier is WindowIdentifier.Inventory)
            ResetSlotUnderCursor();
    }
    
    private void Start()
    {
        BaseWindow.ONWindowClosed += OnInventoryClosed;
        ItemSlot.ONCursorEnterSlot += SetSlotUnderCursor;
        ItemSlot.ONCursorExitSlot += ResetSlotUnderCursor;
    }

    private void OnDestroy()
    {
        BaseWindow.ONWindowClosed -= OnInventoryClosed;
        ItemSlot.ONCursorEnterSlot -= SetSlotUnderCursor;
        ItemSlot.ONCursorExitSlot -= ResetSlotUnderCursor;
    }

    private void Update()
    {
        if (SlotUnderCursor is not null && Input.anyKeyDown)
        {
            SlotUnderCursor.OnKeyDown();
        }
    }

    
}
