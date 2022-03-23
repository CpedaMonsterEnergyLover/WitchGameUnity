using UnityEngine;

public class InteractionKeyListener : KeyListener
{
    public NewItemPicker newItemPicker;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            newItemPicker.Use();
        }
    }

    private void Start()
    {
        SubToEvents();
    }

    private void OnDestroy()
    {
        UnsubFromEvents();
    }
    
    private void OnPlacingMenuOpened(WindowIdentifier window)
    {
        if (window is WindowIdentifier.Placeable or WindowIdentifier.Inventory)
        {
            enabled = false;
            // UnsubFromCursorEvents();
        }
    }

    private void OnPlacingMenuClosed(WindowIdentifier window)
    {
        if (window is WindowIdentifier.Placeable or WindowIdentifier.Inventory)
        {
            enabled = true;
            // SubCursorEvents();
        }
    }

    private void OnCursorEnterUI() => enabled = false;
    private void OnCursorLeaveUI() => enabled = true;

    private void SubToEvents()
    {
        BaseWindow.ONWindowOpened += OnPlacingMenuOpened;
        BaseWindow.ONWindowClosed += OnPlacingMenuClosed;
    }

    private void UnsubFromEvents()
    {
        BaseWindow.ONWindowOpened -= OnPlacingMenuOpened;
        BaseWindow.ONWindowClosed -= OnPlacingMenuClosed;
        // UnsubFromCursorEvents();
    }

    /*private void SubCursorEvents()
    {
        CursorHoverCheck.ONCursorEnterUI += OnCursorEnterUI;
        CursorHoverCheck.ONCursorLeaveUI += OnCursorLeaveUI;
    } 
    
    private void UnsubFromCursorEvents()
    {
        CursorHoverCheck.ONCursorEnterUI -= OnCursorEnterUI;
        CursorHoverCheck.ONCursorLeaveUI -= OnCursorLeaveUI;
    }*/
}
