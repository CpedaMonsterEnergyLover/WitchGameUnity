using System.Collections.Generic;
using UnityEngine;

public class HotbarWindow : BaseWindow
{
    public GameObject arrow;
    public HotbarSlot currentSelectedSlot;
    [ShowOnly]
    public int selectedSlotIndex;
    
    public List<HotbarSlot> slots = new();
    
    public delegate void ItemChangeEvent(ItemSlot slot);
    public static event ItemChangeEvent ONSelectedSlotChanged;


    private void Start()
    {
        selectedSlotIndex = -1;
        SelectSlot(0);
    }
    
    public void SelectSlot(int index)
    {
        if(selectedSlotIndex == index) return;
        
        if (index > 7) index = 0;
        if (index < 0) index = 7;
        selectedSlotIndex = index;
        currentSelectedSlot = slots[index];

        ONSelectedSlotChanged?.Invoke(currentSelectedSlot);
        
        arrow.transform.SetParent(currentSelectedSlot.transform, false);
    }
    
}