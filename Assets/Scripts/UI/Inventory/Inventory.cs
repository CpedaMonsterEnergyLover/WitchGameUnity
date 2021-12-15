using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    #region Singleton

    public static Inventory Instance;

    private void Awake()
    {
        if(Instance is null) Instance = this;
        else Debug.LogError("Found instance of Inventory. You did something wrong.");
    }

    #endregion

    public Transform inventoryTransform;
    public Transform hotBarTransform;

    public GameObject slotSelection;
    public Color selectionColor;

    public InventorySlot itemPicker;
    public InventorySlot SelectedSlot { get; private set; }

    [ShowOnly]
    public int selectedSlotIndex;

    [SerializeReference]
    private List<InventorySlot> _slots;

    public delegate void OnItemChanged();

    public OnItemChanged ONItemChanged;

    // Private fields
    private bool _active = true;

    private void Start()
    {
        _slots.AddRange(hotBarTransform.GetComponentsInChildren<InventorySlot>());
        _slots.AddRange(inventoryTransform.GetComponentsInChildren<InventorySlot>());
        ShowInventory(_active);
        SelectSlot(0);
    }

    private void Update()
    {
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

    public void SelectSlot(int index)
    {
        if (index > 7) index = 0;
        if (index < 0) index = 7;
        
        // Arrow position
        var selectionTransform = slotSelection.GetComponent<RectTransform>();
        var position = slotSelection.transform.localPosition;
        position.x = 18 + 72 * index
                     - slotSelection.transform.parent.GetComponent<RectTransform>().sizeDelta.x / 2;
        selectionTransform.transform.localPosition = position;
        
        // Slot color
        if(SelectedSlot is not null) SelectedSlot.GetComponent<Image>().color = Color.white;
        
        selectedSlotIndex = index;
        SelectedSlot = _slots[index];
        SelectedSlot.GetComponent<Image>().color = selectionColor;

    }
    
    public void AddItem(Item item, int amount)
    {
        foreach (InventorySlot slot in _slots)
        {
            if (slot.Count == 0 || (slot.Item == item && slot.Count < item.maxStack))
            {
                int added = slot.AddItem(item, amount);
                Debug.Log(added);
                amount -= added;
                if (amount <= 0) break;
            }
        }
    }
    
    private void AddItemByIndex(int index, Item item, int amount)
    {
        _slots[index].AddItem(item, amount);
        ONItemChanged?.Invoke();
    }

    public void PickItem(Item item, int count)
    {
        itemPicker.gameObject.SetActive(true);
        itemPicker.AddItem(item, count);
    }

    public void ClearPicker()
    {
        itemPicker.gameObject.SetActive(false);
        itemPicker.enabled = false;
    }

    public void ShowInventory(bool isShown)
    {
        inventoryTransform.parent.gameObject.SetActive(isShown);
        _active = isShown;
    }

    public void ToggleInventory()
    {
        ShowInventory(!_active);
    }
}
