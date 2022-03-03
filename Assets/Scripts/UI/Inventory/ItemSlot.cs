using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item storedItem;
    public int storedAmount;

    public Image itemIcon;
    public Text itemText;
    public bool HasItem => storedItem is not null && storedAmount > 0;

    private Image _image;

    #region UnityMethods

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        _image.color = new Color(0.87f, 0.87f, 0.87f);
        InventoryKeyManager.Instance.slotUnderCursor = this;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        _image.color = Color.white;
        InventoryKeyManager.Instance.slotUnderCursor = null;
    }

    private void Start()
    {
        _image = GetComponent<Image>();
    }

    #endregion
    
    

    #region ClassMethods

    // Добавляет предметы в слот и возвращает, сколько предметов влезло
    public virtual int AddItem(Item item, int amount)
    {
        // Если в слоте уже лежит предмет и пытаются добавить другой - он не добавляется
        if (storedItem is not null && !item.Compare(storedItem)) return 0;

        int canFit = storedItem is null ? item.Data.maxStack : storedItem.Data.maxStack - storedAmount;
        int added = canFit < amount ? canFit : amount;
        storedAmount += added;
        storedItem = item;

        UpdateUI();

        return added;
    }

    // Удаляет предметы из слота
    public virtual void RemoveItem(int amount)
    {
        if (storedItem is null || amount <= 0) return;

        storedAmount -= amount;
        itemText.text = storedAmount.ToString();
        itemText.enabled = storedAmount > 1;
        if (storedAmount <= 0)
        {
            Clear();
            Inventory.Instance.UpdateUI();
        }

        UpdateUI();
    }

    public virtual void UpdateUI()
    {
        // Если предмет был последний
        if (storedItem == null || storedAmount <= 0)
        {
            itemIcon.enabled = false;
            itemText.enabled = false;
            if(InventoryKeyManager.Instance.slotUnderCursor == this) 
                Tooltip.Instance.SetEnabled(false);
        }
        // Если не последний
        else {
            itemIcon.sprite = storedItem.Data.icon;
            itemIcon.enabled = true;
        
            itemText.text = storedAmount.ToString();
            itemText.enabled = storedAmount > 1;
        }
        
    }

    public virtual void OnKeyDown()
    {
    }
    
    #endregion



    #region Utils
    
    public void Shake()
    {
        StartCoroutine(Shake(0.75f, 30f));

    }
    
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
    
    public virtual void Clear()
    {
        storedAmount = 0;
        storedItem = null;
        UpdateUI();
    }

    #region Utils

    public static int NameComparator(ItemSlot x, ItemSlot y) =>
        !x.HasItem && !y.HasItem ? 0 :
        !x.HasItem ? 1 :
        !y.HasItem ? -1 :
        string.Compare(x.storedItem.Data.name, y.storedItem.Data.name, StringComparison.Ordinal);
    
    public static int TypeComparator(ItemSlot x, ItemSlot y) =>
        !x.HasItem && !y.HasItem ? 0 :
        !x.HasItem ? 1 :
        !y.HasItem ? -1 :
        x.storedItem.Data.identifier.type.CompareTo(y.storedItem.Data.identifier.type);

    public static int AmountComparator(ItemSlot x, ItemSlot y) =>
        !x.HasItem && !y.HasItem ? 0 :
        !x.HasItem ? 1 :
        !y.HasItem ? -1 :
        y.storedAmount.CompareTo(x.storedAmount);

    #endregion



    #endregion
}
