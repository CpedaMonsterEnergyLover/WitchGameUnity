using UnityEngine;
using UnityEngine.UI;

public class PlaceableWindow : BaseWindow
{
    public GameObject closeButton;
    public Image itemIcon;
    public Text itemText;
    public Text itemAmount;

    private GameObject _instantiatedPrefab;
    private ItemSlot _referredSlot;

    private bool _wasInventoryClosed;
    private bool _wasCraftingWindowClosed;
    private bool _wasHotbarClosed;

    public void ShowMenu(CraftingSlot slot)
    {
        if(slot.recipe.result.item is not PlaceableData placeableData) return;
        _instantiatedPrefab = Instantiate(placeableData.prefab);
        
        
        /*CloseInventory();
        CloseHotbar();
        CloseCraftingMenu();*/
        closeButton.SetActive(true);
        gameObject.SetActive(true);
    }

    /*private void CloseInventory()
    {
        if (InventoryWindow.Instance.isActiveAndEnabled)
        {
            _wasInventoryClosed = true;
            InventoryWindow.Instance.SetActive(false);
        }
    }

    private void CloseHotbar()
    {
        if (WindowManager.Instance.IsActive(WindowIdentifier.Hotbar))
        {
            _wasHotbarClosed = true;
            HotbarWindow.Instance.SetActive(false);
        }
    }

    private void CloseCraftingMenu()
    {
        if (CraftingWindow.Instance.isActiveAndEnabled)
        {
            _wasCraftingWindowClosed = true;
            CraftingWindow.Instance.SetActive(false);
        }
    }
    
    public void Close()
    {
        gameObject.SetActive(false);
        // if(_wasHotbarClosed) HotbarWindow.Instance.SetActive(true);
        if(_wasCraftingWindowClosed) CraftingWindow.Instance.SetActive(true);
        if(_wasInventoryClosed) InventoryWindow.Instance.SetActive(true);
    }

    private void HideOtherWindows()
    {
        
    }
    */
}
