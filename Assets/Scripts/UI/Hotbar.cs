using System;
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
    
    public Transform hotBarTransform;
    
    public GameObject arrow;
    public Color selectionColor;
    public InventorySlot currentSelectedSlot;
    [ShowOnly]
    public int selectedSlotIndex;

    public List<InventorySlot> slots = new();
    
    public delegate void ItemChangeEvent(InventorySlot slot);
    public static event ItemChangeEvent ONSelectedSlotChanged;

    private void Start()
    {
        Inventory.ONInventoryOpened += HideSelection;
        Inventory.ONInventoryClosed += ShowSelection;
        slots.AddRange(hotBarTransform.GetComponentsInChildren<InventorySlot>());
        selectedSlotIndex = -1;
        SelectSlot(0);
    }

    private void Update()
    {
        if(Inventory.Instance.IsActive) return;
        SelectFromWheel();
        SelectFromKeyboard();
    }


    private void SelectFromWheel()
    {
        // Mouse wheel
        float wheel = Input.GetAxisRaw("Mouse ScrollWheel");
        if (wheel < - 0)
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
        
        //TODO: да какова хуя цвет не меняется на старте
        // Reset previous slot color
        if (currentSelectedSlot is not null) currentSelectedSlot.GetComponent<Image>().color = Color.white;
        selectedSlotIndex = index;
        currentSelectedSlot = slots[index];
        // Apply new color
        currentSelectedSlot.GetComponent<Image>().color = selectionColor;

        ONSelectedSlotChanged?.Invoke(currentSelectedSlot);
        
        // Arrow position
        var selectionTransform = arrow.GetComponent<RectTransform>();
        var position = arrow.transform.localPosition;
        position.x = 18 + 72 * index
                     - arrow.transform.parent.GetComponent<RectTransform>().sizeDelta.x / 2;
        selectionTransform.transform.localPosition = position;
        
        
    }

    private void ShowSelection()
    {
        arrow.SetActive(true);
        if (currentSelectedSlot is not null) currentSelectedSlot.GetComponent<Image>().color = selectionColor;
    }

    private void HideSelection()
    {
        arrow.SetActive(false);
        if (currentSelectedSlot is not null) currentSelectedSlot.GetComponent<Image>().color = Color.white;
    }
    
}
