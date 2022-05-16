using System.Collections.Generic;
using UnityEngine;

public class HotbarWindow : BaseWindow
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private List<HotbarSlot> slots = new();
    
    public HotbarSlot SelectedSlot { get; private set; }

    public delegate void SelectedSlotEvent(ItemSlot slot);
    public static event SelectedSlotEvent ONSelectedSlotChanged;
    
    private void Start()
    {
        for (var i = slots.Count - 1; i >= 0; i--) slots[i].Index = i;
        SelectedSlot = slots[0];
        ONSelectedSlotChanged?.Invoke(SelectedSlot);
    }

    public HotbarSlot GetSlot(int index) => slots[index];
    
    public void SelectSlot(int index)
    {
        if(SelectedSlot.Index == index) return;
        if (index > 7) index = 0;
        if (index < 0) index = 7;
        SelectedSlot = slots[index];
        ONSelectedSlotChanged?.Invoke(SelectedSlot);
        arrow.transform.SetParent(SelectedSlot.transform, false);
    }
    
}