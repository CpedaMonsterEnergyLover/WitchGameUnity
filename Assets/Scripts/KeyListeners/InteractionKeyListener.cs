using UnityEngine;

public class InteractionKeyListener : MonoBehaviour
{
    public NewItemPicker newItemPicker;


    private void OnEnable()
    {
        SubToEvents();
    }

    private void OnDisable()
    {
        UnsubFromEvents();
    }

    private void OnDestroy()
    {
        UnsubFromEvents();
    }

    private void Update()
    {
        ListenToKeyboard();
    }
    
    private void ListenToKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (newItemPicker.gameObject.activeInHierarchy)
                if (newItemPicker.itemSlotGO.activeInHierarchy)
                    newItemPicker.UseItem();
                else
                    newItemPicker.UseHand();
            else
                newItemPicker.UseHand();
        }
    }

    private void OnInventoryOpened(WindowIdentifier window)
    {
        if(window == WindowIdentifier.Inventory)
            enabled = false;
    }

    private void OnInventoryClosed(WindowIdentifier window)
    {
        if(window == WindowIdentifier.Inventory)
            enabled = true;
    }
    
    private void SubToEvents()
    {
        BaseWindow.ONWindowOpened += OnInventoryOpened;
        BaseWindow.ONWindowClosed += OnInventoryClosed;
    }

    private void UnsubFromEvents()
    {
        BaseWindow.ONWindowOpened -= OnInventoryOpened;
        BaseWindow.ONWindowClosed -= OnInventoryOpened;
    }
}
