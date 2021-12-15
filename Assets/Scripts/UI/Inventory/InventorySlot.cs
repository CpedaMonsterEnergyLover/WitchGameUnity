using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerDownHandler/*, IDragHandler, IBeginDragHandler, IEndDragHandler*/
{
    public ItemType SlotType;
    [SerializeField] 
    public Item StoredItem;
    [SerializeField] 
    public int StoredCount = 0;

    public Image itemIcon;
    public Text itemText;
    public bool HasItem => StoredItem is not null && StoredCount > 0;
    
    public delegate void BagEquipEvent(Bag bag);
    public static event BagEquipEvent ONBagEquip;
    
    public delegate void BagUnequipEvent(Bag bag);
    public static event BagUnequipEvent ONBagUnequip;
    
    public delegate void BagEquipDeniedEvent(Bag bag);
    public static event BagEquipDeniedEvent ONBagEquipDenied;

    private Coroutine _routine;

    #region UnityMethods

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
        if (SlotType == ItemType.Bag && item.type == ItemType.Bag)
                if (!ManageBagEquip((Bag) item)) return 0;
        
        // Если в слоте уже лежит предмет и пытаются добавить другой - он не добавляется
        if (StoredItem is not null && StoredItem != item) return 0;
        // Если в слот нельзя класть предметы этого типа - они не добавляются
        if (SlotType != ItemType.Any && SlotType != item.type) return 0;
        


        int canFit = StoredItem is null ? item.maxStack : StoredItem.maxStack - StoredCount;
        int added = canFit < amount ? canFit : amount;
        StoredCount += added;
        StoredItem = item;

        UpdateUI();

        return added;
    }


    // Удаляет предметы из слота
    public void RemoveItem(int amount)
    {
        if (StoredItem is null || amount == 0) return;
        if (StoredCount - amount <= 0)
        {
            StoredCount = 0;
            itemIcon.enabled = false;
            StoredItem = null;
            itemText.enabled = false;
        }
        else
        {
            StoredCount -= amount;
            itemText.text = StoredCount.ToString();
        }
    }

    private bool ManageBagEquip(Bag bag)
    {
        Bag storedBag = (Bag) StoredItem;
        if (HasItem)
        {
            if (storedBag.IsEmpty())
            {
                ONBagUnequip?.Invoke(storedBag);
                ONBagEquip?.Invoke(bag);
                return true;
            }

            ONBagEquipDenied?.Invoke(storedBag);
            return false;
        }
        ONBagEquip?.Invoke(bag);
        return true;
    }

    private bool ManageBagUnequip(Bag bag)
    {
        Bag storedBag = (Bag) StoredItem;
        if (storedBag.IsEmpty())
        {
            ONBagUnequip?.Invoke(storedBag);
            return true;
        }
        ONBagEquipDenied?.Invoke(storedBag);
        return false;
    }
    
    // При нажатии на ячейку инвентаря
    public void OnPointerDown(PointerEventData eventData)
    {
        bool leftClick = eventData.button == PointerEventData.InputButton.Left;
        bool rightClick = eventData.button == PointerEventData.InputButton.Right;
        if (eventData.button == PointerEventData.InputButton.Middle) return;

        // Если игрок ничего не перетаскивает он может взять из слота прнедмет
        var picker = Inventory.Instance.itemPicker;
        int pickedAmount = 0;
        bool hasPickedItem = picker.StoredItem is not null;
        bool heldShift = eventData.currentInputModule.input.GetAxisRaw("Shift") != 0;
        bool pickedSameItem = StoredItem == picker.StoredItem;

        if (leftClick)
        {
            pickedAmount = StoredCount;
        } else if (rightClick)
        {
            if (heldShift) pickedAmount = StoredCount / 2 == 0 ? 1 : StoredCount / 2;
            else pickedAmount = 1;
        } 
        
        // Если игрок не перетаскивает предмет
        if (!hasPickedItem)
        {
            // И если в слоте нет предмета, ничего не происходит
            if (!HasItem) return;

            // Проверка на сумку
            // Если слот для сумок и пытаются убрать сумку
            if (SlotType == ItemType.Bag && StoredItem.type == ItemType.Bag)
                if (!ManageBagUnequip((Bag) StoredItem)) return;
            
            Inventory.Instance.PickItem(StoredItem, pickedAmount);
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
                            pickedAmount = StoredCount;
                        }
                        int added = picker.AddItem(StoredItem, pickedAmount);
                        RemoveItem(added);
                        return;
                    }
                    int added2 = AddItem(picker.StoredItem, picker.StoredCount);
                    picker.RemoveItem(added2);
                    if (picker.StoredCount <= 0) Inventory.Instance.ClearPicker();
                }
                else
                {
                    // Если слот для сумок и пытаются убрать сумку
                    if (SlotType == ItemType.Bag && StoredItem.type == ItemType.Bag)
                        if (!ManageBagUnequip((Bag) StoredItem)) return;
                    
                    Item tempItem = picker.StoredItem;
                    int tempCount = picker.StoredCount;
                    picker.Clear();
                    picker.AddItem(StoredItem, StoredCount);
                    Clear();
                    AddItem(tempItem, tempCount);
                }
            }
            else
            {
                if (leftClick) pickedAmount = picker.StoredCount;
                else if (rightClick) pickedAmount = 1;
                
                int added = AddItem(picker.StoredItem, pickedAmount);
                picker.RemoveItem(added);
                if (picker.StoredCount <= 0) Inventory.Instance.ClearPicker();
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
            Debug.Log(angle);
            // itemIcon.GetComponent<SpriteRenderer>().color = new Color(1f, 0.08f, 0f);
            yield return null;
        }
        // itemIcon.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }
    
    public void Clear()
    {
        RemoveItem(StoredCount);
    }

    public void UpdateUI()
    {
        itemIcon.sprite = StoredItem.icon;
        itemIcon.enabled = true;
        
        if (StoredCount > 1)
        {
            itemText.text = StoredCount.ToString();
            itemText.enabled = true;
        }
    }


    #endregion
}
