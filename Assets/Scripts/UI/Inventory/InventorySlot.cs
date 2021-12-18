using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ItemType SlotType;
    [SerializeField]
    public Item storedItem;
    [SerializeField] 
    public int StoredAmount = 0;

    public Image itemIcon;
    public Text itemText;
    public bool HasItem => storedItem is not null && StoredAmount > 0;
    
    public delegate void BagEquipEvent(Bag bag);
    public static event BagEquipEvent ONBagEquip;
    
    public delegate void BagUnequipEvent(Bag bag);
    public static event BagUnequipEvent ONBagUnequip;
    
    public delegate void BagEquipDeniedEvent(Bag bag);
    public static event BagEquipDeniedEvent ONBagUnEquipDenied;

    private Coroutine _routine;

    #region UnityMethods

    private void Awake()
    {
        storedItem = null;
    }

    private void Start()
    {
        GetComponent<Image>().color = Inventory.Instance.SlotColor(SlotType);
    }

    #endregion



    #region ClassMethods

    // Добавляет предметы в слот и возвращает, сколько предметов влезло
    public int AddItem(Item item, int amount)
    {
        // Если слот для сумок и пытаются поместить сумку
        if (!TryEquipBag(item)) return 0;
        
        // Если в слоте уже лежит предмет и пытаются добавить другой - он не добавляется
        if (storedItem is not null && !item.Compare(storedItem)) return 0;
        // Если в слот нельзя класть предметы этого типа - они не добавляются
        if (SlotType != ItemType.Any && SlotType != item.Type) return 0;

        int canFit = storedItem is null ? item.Data.maxStack : storedItem.Data.maxStack - StoredAmount;
        int added = canFit < amount ? canFit : amount;
        StoredAmount += added;
        storedItem = item;

        UpdateUI();

        return added;
    }


    // Удаляет предметы из слота
    public void RemoveItem(int amount)
    {
        if (storedItem is null || amount <= 0)
        {
            return;
        }
        if (StoredAmount - amount <= 0)
        {
            StoredAmount = 0;
            itemIcon.enabled = false;
            storedItem = null;
            itemText.enabled = false;
        }
        else
        {
            StoredAmount -= amount;
            if (StoredAmount <= 1) itemText.enabled = false;
            itemText.text = StoredAmount.ToString();
        }

                // UpdateUI();
    }

    private bool TryEquipBag(Item item)
    {
        if (SlotType != ItemType.Bag ||
            item.Type != ItemType.Bag) return true;

        Bag bag = (Bag) item;
        Bag storedBag = (Bag) storedItem;
        
        if (HasItem)
        {
            if (storedBag.InstanceData.IsEmpty())
            {
                ONBagUnequip?.Invoke(storedBag);
                ONBagEquip?.Invoke(bag);
                return true;
            }

            ONBagUnEquipDenied?.Invoke(storedBag);
            return false;
        }
        ONBagEquip?.Invoke(bag);
        return true;
    }

    private bool TryUnequipBag()
    {
        if (SlotType != ItemType.Bag ||
            storedItem.Data.identifier.type != ItemType.Bag) return true;

        if (ItemPicker.Instance.itemSlot.HasItem) return false;

        Bag storedBag = (Bag) storedItem;

        if (storedBag.InstanceData.IsEmpty())
        {
            ONBagUnequip?.Invoke(storedBag);
            return true;
        }
        ONBagUnEquipDenied?.Invoke(storedBag);
        return false;
    }
    
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!HasItem || ItemPicker.Instance.itemSlot.HasItem) return;
        Tooltip.Instance.SetData(storedItem.GetToolTipData());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.Instance.SetEnabled(false);
    }
    
    // При нажатии на ячейку инвентаря
    public void OnPointerDown(PointerEventData eventData)
    {
        bool leftClick = eventData.button == PointerEventData.InputButton.Left;
        bool rightClick = eventData.button == PointerEventData.InputButton.Right;
        if (eventData.button == PointerEventData.InputButton.Middle) return;

        // Если игрок ничего не перетаскивает он может взять из слота прнедмет
        int pickedAmount = 0;
        bool heldShift = eventData.currentInputModule.input.GetAxisRaw("Shift") != 0;
        InventorySlot picker = ItemPicker.Instance.itemSlot;
        bool hasPickedItem = picker.HasItem;
        bool pickedSameItem = HasItem &&  storedItem.Compare(picker.storedItem);

        if (leftClick)
        {
            pickedAmount = StoredAmount;
        } else if (rightClick)
        {
            if (heldShift) pickedAmount = StoredAmount / 2 == 0 ? 1 : StoredAmount / 2;
            else pickedAmount = 1;
        } 
        
        // Если игрок не перетаскивает предмет
        if (!hasPickedItem)
        {
            // И если в слоте нет предмета, ничего не происходит
            if (!HasItem) return;

            // Проверка на сумку
            if (!TryUnequipBag()) return;
            
            ItemPicker.Instance.SetItem(storedItem, pickedAmount);
            RemoveItem(pickedAmount);
        }
        // Если игрок перетаскивает предмет
        else
        {
            if (HasItem)
            {
                // Если взят тот же предмет, что и в слоте
                if (pickedSameItem)
                {
                    if (rightClick)
                    {
                        if (heldShift)
                        {
                            pickedAmount = StoredAmount;
                        }
                        int added = ItemPicker.Instance.SetItem(storedItem, pickedAmount);
                        RemoveItem(added);
                        return;
                    }
                    int added2 = AddItem(picker.storedItem, picker.StoredAmount);
                    picker.RemoveItem(added2);
                    if (picker.StoredAmount <= 0) ItemPicker.Instance.Clear();
                }
                else
                {
                    // Проверка на сумку
                    if (!TryUnequipBag()) return;

                    Item tempItem = picker.storedItem;
                    int tempCount = picker.StoredAmount;
                    picker.Clear();
                    ItemPicker.Instance.SetItem(storedItem, pickedAmount);
                    Clear();
                    AddItem(tempItem, tempCount);
                }
            }
            else
            {
                if (leftClick) pickedAmount = picker.StoredAmount;
                else if (rightClick) pickedAmount = 1;
                
                int added = AddItem(picker.storedItem, pickedAmount);
                picker.RemoveItem(added);
                if (picker.StoredAmount <= 0) ItemPicker.Instance.Clear();
            }
        }
    }

    public void Shake()
    {
        if (_routine is not null) StopCoroutine(_routine);
        _routine = StartCoroutine(Shake(0.75f, 30f));

    }

    #endregion

    #region Utils

    private IEnumerator Shake(float duration, float speed)
    {
        float t = 0.0f;
        while ( t  < duration )
        {
            t += Time.deltaTime;
            float angle = Mathf.Sin(t * speed) * 5; 
            itemIcon.transform.rotation  = Quaternion.AngleAxis(angle, Vector3.forward);
            yield return null;
        }
    }
    
    public void Clear()
    {
        RemoveItem(StoredAmount);
    }

    public void UpdateUI()
    {
        itemIcon.sprite = storedItem.Data.icon;
        itemIcon.enabled = true;
        
        if (StoredAmount > 1)
        {
            itemText.text = StoredAmount.ToString();
            itemText.enabled = true;
        }
        else
        {
            itemText.enabled = false;
        }
    }


    #endregion

}
