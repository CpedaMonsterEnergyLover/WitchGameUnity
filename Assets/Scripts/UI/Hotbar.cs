using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    #region Singleton

    public static Hotbar Instance;

    private void Awake()
    {
        if (Instance is null) Instance = this;
        else Debug.LogError("Found multiple instances of Hotbar");
    }

    #endregion
    
    public GameObject arrow;
    public Color selectionColor;
    public HotbarSlot currentSelectedSlot;
    [ShowOnly]
    public int selectedSlotIndex;
    
    public List<HotbarSlot> slots = new();
    
    public delegate void ItemChangeEvent(ItemSlot slot);
    public static event ItemChangeEvent ONSelectedSlotChanged;

    private void Start()
    {
        /*Inventory.ONInventoryOpened += HideSelection;
        Inventory.ONInventoryClosed += ShowSelection;*/

        selectedSlotIndex = -1;
        SelectSlot(0);
    }

    private void Update()
    {
        SelectFromWheel();
        SelectFromKeyboard();
    }


    private void SelectFromWheel()
    {
        // Mouse wheel
        float wheel = Input.GetAxisRaw("Mouse ScrollWheel");
        if (wheel < 0)
        {
            SelectSlot(selectedSlotIndex + 1);
        }
        else if (wheel > 0)
        {
            SelectSlot(selectedSlotIndex - 1);
        }
    }

    private void SelectFromKeyboard()
    {
        // Keyboard
        for (int i = 0; i < 8; i++)
            if (Input.GetKeyDown((i + 1).ToString()))
                SelectSlot(i);
    }
    
    public void SelectSlot(int index)
    {
        if(selectedSlotIndex == index) return;
        
        if (index > 7) index = 0;
        if (index < 0) index = 7;
        // Reset previous slot color
        if (currentSelectedSlot is not null) currentSelectedSlot.GetComponent<Image>().color = Color.white;
        selectedSlotIndex = index;
        currentSelectedSlot = slots[index];
        // Apply new color
        currentSelectedSlot.GetComponent<Image>().color = selectionColor;

        ONSelectedSlotChanged?.Invoke(currentSelectedSlot);
        
        // Arrow position
        arrow.transform.SetParent(currentSelectedSlot.transform, false);
    }

    public void ConsumeItem(int amount)
    {
        currentSelectedSlot.ReferredSlot.RemoveItem(amount);   
    }

}
