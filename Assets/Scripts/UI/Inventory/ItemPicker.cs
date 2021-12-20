using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPicker : MonoBehaviour
{
    #region Singleton

    public static ItemPicker Instance;

    private void Awake()
    {
        if (Instance is null) Instance = this;
        else Debug.LogError("Found multiple instances of itempicker");
    }

    #endregion


    public bool PickedFromInventory => Inventory.Instance.IsActive;
    
    public Camera playerCamera;
    public Color previewDenyColor;
    public Color previewAllowColor;
    public InventorySlot itemSlot;
    public Item Item => itemSlot.storedItem;
    
    private GameObject _interactablePreview;
    public bool _previewActive;
    // Тайл, над которым находится курсор с пикером
    private WorldTile _tileUnderPicker;
    private InventorySlot _pickedFrom;
    private readonly List<SpriteRenderer> _previewRenderers = new();
    
    public bool IsPlaceable => Item is IPlaceable;
    public bool IsUsable => Item is IUsable;
    public bool IsConsumable => Item is IConsumable;

    private void Start()
    {
        CursorHoverCheck.ONCursorEnterUI += ONCursorEnterUI;
        CursorHoverCheck.ONCursorLeaveUI += ONCursorLeaveUI;
        Hotbar.ONSelectedSlotChanged += ONHotBarSelectedSlotChanged;
        Inventory.ONInventoryClosed += OnInventoryClosed;
        Inventory.ONInventoryOpened += OnInventoryOpened;
        gameObject.SetActive(false);
    }

    public int SetItem(InventorySlot fromSlot, int amount)
    {
        _pickedFrom = fromSlot;
        gameObject.SetActive(true);
        int added = itemSlot.AddItem(fromSlot.storedItem, amount);
        Tooltip.Instance.SetEnabled(false);

        if (IsPlaceable)
        {
            _interactablePreview = 
                Instantiate(Interactable.GetPrefab(Item.Data.placedObject.identifier));
            _interactablePreview.SetActive(false);
            CachePreviewSpriteRenderers();
        }

        return added;
    }

    public void Clear()
    {
        gameObject.SetActive(false);
        itemSlot.Clear();
        if (_interactablePreview is not null) DestroyImmediate(_interactablePreview);
        _previewRenderers.Clear();
        _previewActive = false;
        _tileUnderPicker = null;
        CursorManager.Instance.ResetMode();
        if(CursorManager.Instance.InMode(CursorMode.HoverUI)) Tooltip.Instance.SetEnabled(true);

    }
    
    private void OnEnable()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 5;
        /*mousePosition.x -= 34;
        mousePosition.y += 34;*/
        transform.position = mousePosition;
    }
    
    private void Update()
    {
        GetTileUnderPicker();
        UpdatePosition();
        
        CursorManager.Instance.Mode = CursorMode.HoldItem;
        
        bool clicked = Input.GetMouseButtonDown(0) && !CursorManager.Instance.IsOverUI;
        
        if (IsPlaceable)
        {
            UpdateInteractablePreview();
            if(clicked) TryPlaceItem();
        }else if (IsConsumable)
        {
            UpdateConsumablePreview();
            if(clicked) TryConsumeItem();
        } else if (IsUsable)
        {
            UpdateUsablePreview();
            if(clicked) TryUseItem();
        }
        else
        {
            transform.position += new Vector3(-34, +34, 0);
        }
    }

    private void TryPlaceItem()
    {
        if(_tileUnderPicker is null) return;
        IPlaceable pickedPlaceable = (IPlaceable)Item;
        if (pickedPlaceable.AllowPlace(_tileUnderPicker))
        {
            // Использует инвок чтобы текущий клик сразу не засчитался на заспавн. объекте
            Invoke(nameof(PlacePickedItem), 0.1f);
            if(!Inventory.Instance.IsActive) Hotbar.Instance.currentSelectedSlot.RemoveItem(1);
        }
    }

    private void TryConsumeItem()
    {
        if (_tileUnderPicker is null) return;
        IConsumable pickedConsumable = (IConsumable)Item;
        if (pickedConsumable.AllowConsume())
        {
            pickedConsumable.Consume();
            itemSlot.RemoveItem(1);
            if (itemSlot.storedAmount <= 0) Clear();
            if(!Inventory.Instance.IsActive) Hotbar.Instance.currentSelectedSlot.RemoveItem(1);
        }
    }
    
    public void PlacePickedItem()
    {
        ((IPlaceable) Item).Place(_tileUnderPicker);
        itemSlot.RemoveItem(1);
        if (itemSlot.storedAmount <= 0) Clear();
    }

    public void TryUseItem()
    {
        if(_tileUnderPicker is null) return;
        IUsable pickedUsable = (IUsable)Item;
        if (pickedUsable.AllowUse(_tileUnderPicker))
            pickedUsable.Use(_tileUnderPicker);
    }

    private void GetTileUnderPicker()
    {
        Vector3 mouseWorldPos = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        Vector3Int gridPos = Vector3Int.FloorToInt(mouseWorldPos);

        if (WorldManager.CoordsBelongsToWorld(gridPos.x, gridPos.y))
        {
            _tileUnderPicker = WorldManager.WorldData.GetTile(gridPos.x, gridPos.y);
        }
    }

    private void UpdateInteractablePreview()
    {
        if(!_previewActive || _tileUnderPicker is null) return;
        
        _interactablePreview.transform.position = _tileUnderPicker.position + new Vector3(0.5f, 0.5f, 0);
        _previewRenderers.ForEach(r =>
        {
            r.color = ((IPlaceable)Item).AllowPlace(_tileUnderPicker) ? previewAllowColor : previewDenyColor;
        });
    }

    private void UpdateConsumablePreview()
    {
        if (((IConsumable) Item).AllowConsume())
        {
            itemSlot.itemIcon.color = Color.white;
            transform.position += new Vector3(-34, +34, 0);
        }
        else
        {
            itemSlot.itemIcon.color = new Color(1, 1, 1, 0.5f);
        }
    }

    private void UpdateUsablePreview()
    {
        if (((IUsable) Item).AllowUse(_tileUnderPicker) && 
            !CursorManager.Instance.IsOverUI)
        {
            itemSlot.itemIcon.color = Color.white;
            // transform.position += new Vector3(+40, -40, 0);
            transform.position += new Vector3(10, -10, 0);
        }
        else
        {
            if(!PickedFromInventory) itemSlot.itemIcon.color = new Color(1, 1, 1, 0f);
            else
            {
                itemSlot.itemIcon.color = new Color(1, 1, 1, 1f);
                transform.position += new Vector3(-34, +34, 0);
            }
        }
    }

    private void CachePreviewSpriteRenderers()
    {
        if (_interactablePreview is null) return;
        foreach (SpriteRenderer spriteRenderer in _interactablePreview.GetComponentsInChildren<SpriteRenderer>())
        {
            _previewRenderers.Add(spriteRenderer);
        }
    }

    private void ONCursorLeaveUI()
    {
        if(!itemSlot.HasItem) return;
        if (IsPlaceable)
        {
            if (_interactablePreview is not null) ShowInteractablePreview();
        } else
        {
            itemSlot.UpdateUI();
        }
    }

    private void ShowInteractablePreview()
    {
        _interactablePreview.SetActive(true);
        itemSlot.itemIcon.enabled = false;
        _previewActive = true;
    }

    private void ONCursorEnterUI()
    {
        if (IsPlaceable && _interactablePreview is not null)
        {
            _previewActive = false;
            _interactablePreview.SetActive(false);

            if (PickedFromInventory)
                itemSlot.itemIcon.enabled = true;
            else {
                itemSlot.itemIcon.enabled = false;
                itemSlot.itemText.enabled = false;
            }
        } else
        {
            if (!PickedFromInventory)
            {
                itemSlot.itemIcon.enabled = false;
                itemSlot.itemText.enabled = false;
            }
        }
    }

    // Когда меняется предмет в хотбаре
    private void ONHotBarSelectedSlotChanged(InventorySlot slot)
    {
        // Очищает пикер от предыдущего предмета
        if (itemSlot.HasItem) Clear();
        UpdatePreview(slot);
    }

    // Засовывает в пикер предмет из слота и обновляет его превью в мире
    public void UpdatePreview(InventorySlot slot)
    {
        if (!slot.HasItem) return;
        bool canPlace = slot.storedItem is IPlaceable;
        bool canUse = slot.storedItem is IUsable;
        bool canConsume = slot.storedItem is IConsumable;
        
        // Если в слоте хотбара есть предмет, и его можно использовать или поставить
        if (canConsume || canPlace || canUse)
        {
            SetItem(slot, slot.storedAmount);
            if (CursorManager.Instance.InMode(CursorMode.InWorld))
            {
                if (canPlace) ShowInteractablePreview();
            }
        }
    }
    
    private void OnInventoryOpened()
    {
        // При открытии инвентаря, если в ItemPicker есть предмет, он чистится
        if (itemSlot.HasItem) Clear();
    }

    private void OnInventoryClosed()
    {
        // При закрытии инвентаря, ItemPicker чистится, а его содержимое возвращается обратно
        // В тот же слот, откуда было взято
        if (itemSlot.HasItem)
        {
            _pickedFrom.AddItem(itemSlot.storedItem, itemSlot.storedAmount);
            Clear();
        }
        // Затем обновляет превью предмета из хотбара
        // Использует инвок потому что до конца кадра инвентарь не будет закрыт
        Invoke(nameof(ToInvoke), 0.11f);
    }
    
    private void ToInvoke() => UpdatePreview(Hotbar.Instance.currentSelectedSlot);
    
}
