using UnityEngine;

public class PlacingKeyListener : KeyListener
{
    public PlaceableWindow placeableWindow;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            placeableWindow.Place();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            placeableWindow.SetActive(false);
        }
    }
    
    private void Start()
    {
        SubToEvents();
        enabled = false;
    }

    private void OnDestroy()
    {
        UnsubFromEvents();
    }
    
    private void Enable()
    {
        enabled = true;
        // SubCursorEvents();
    }

    private void OnPlacingMenuOpened(WindowIdentifier window)
    {
        if (window == WindowIdentifier.Placeable)
            Invoke(nameof(Enable), 0.25f);
    }

    private void OnPlacingMenuClosed(WindowIdentifier window)
    {
        if (window == WindowIdentifier.Placeable)
        {
            CancelInvoke();
            enabled = false;
            // UnsubFromCursorEvents();
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
