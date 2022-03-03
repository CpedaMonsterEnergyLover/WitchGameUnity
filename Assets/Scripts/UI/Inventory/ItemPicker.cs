/*
using System.Collections.Generic;
using UnityEngine;

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
    public ItemSlot itemSlot;
    public Item Item => itemSlot.storedItem;
    
    private GameObject _interactablePreview;
    public bool _previewActive;
    // Тайл, над которым находится курсор с пикером
    [SerializeField]
    private WorldTile _tileUnderPicker;
    private readonly List<SpriteRenderer> _previewRenderers = new();
    
    public bool IsPlaceable => Item is IPlaceable;
    public bool IsUsable => Item is IUsableOnTile;
    public bool IsConsumable => Item is IConsumable;

    private void Start()
    {
        Hotbar.ONSelectedSlotChanged += ONHotBarSelectedSlotChanged;
        /*CursorHoverCheck.ONCursorEnterUI += ONCursorEnterUI;
        CursorHoverCheck.ONCursorLeaveUI += ONCursorLeaveUI;
        Inventory.ONInventoryClosed += OnInventoryClosed;
        Inventory.ONInventoryOpened += OnInventoryOpened;
        #1#
        gameObject.SetActive(false);
    }

    public int SetItem(ItemSlot fromSlot, int amount)
    {

        gameObject.SetActive(true);
        int added = itemSlot.AddItem(fromSlot.storedItem, amount);
        Tooltip.Instance.SetEnabled(false);
        
        if (IsPlaceable)
        {
            _interactablePreview = 
                Instantiate(((IPlaceable)Item).GetPrefab());
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
        if(CursorManager.Instance.Mode == CursorMode.HoverUI) Tooltip.Instance.SetEnabled(true);

    }
    
    private void OnEnable()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 5;
        transform.position = mousePosition;
        if (IsPlaceable)
            UpdateInteractablePreview();
        else if (IsConsumable)
            UpdateConsumablePreview();
        else if (IsUsable)
            UpdateUsablePreview();
        else
            transform.position += new Vector3(-34, +34, 0);
    }
    
    // TODO: добавить для IConsumable разрешенные цели и превью
    private void Update()
    {
        // CursorManager.Instance.Mode = CursorMode.HoldItem;

        GetTileUnderPicker();
        UpdatePosition();
        
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && !CursorManager.IsOverUI)
        {
            if (IsPlaceable)
                TryPlaceItem();
            else if (IsConsumable)
                TryConsumeItem();
            else if (IsUsable)
                TryUseItem();
        }
    }

    private void TryPlaceItem()
    {
        if(_tileUnderPicker is null) return;
        IPlaceable pickedPlaceable = (IPlaceable)Item;
        if (pickedPlaceable.AllowUse(tile: _tileUnderPicker))
        {
            // Использует инвок чтобы текущий клик сразу не засчитался на заспавн. объекте
            PlacePickedItem();
            if(!Inventory.Instance.IsActive) Hotbar.Instance.currentSelectedSlot.RemoveItem(1);
        }
    }

    private void TryConsumeItem()
    {
        if (_tileUnderPicker is null) return;
        IConsumable pickedConsumable = (IConsumable)Item;
        if (pickedConsumable.AllowUse())
        {
            pickedConsumable.Use();
            itemSlot.RemoveItem(1);
            if (itemSlot.storedAmount <= 0) Clear();
            if(!Inventory.Instance.IsActive) Hotbar.Instance.currentSelectedSlot.RemoveItem(1);
        }
    }
    
    public void PlacePickedItem()
    {
        if (_tileUnderPicker is null) return;
        ((IPlaceable) Item).Use(tile: _tileUnderPicker);
        itemSlot.RemoveItem(1);
        if (itemSlot.storedAmount <= 0) Clear();
    }

    public void TryUseItem()
    {
        if(_tileUnderPicker is null) return;
        IUsableOnTile pickedUsableOnTile = (IUsableOnTile)Item;
        if (pickedUsableOnTile.AllowUse(tile: _tileUnderPicker))
            pickedUsableOnTile.Use(tile: _tileUnderPicker);
    }

    private void GetTileUnderPicker()
    {
        Vector3 mouseWorldPos = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        Vector3Int gridPos = Vector3Int.FloorToInt(mouseWorldPos);

        _tileUnderPicker = WorldManager.Instance.CoordsBelongsToWorld(gridPos.x, gridPos.y) ? 
            WorldManager.Instance.WorldData.GetTile(gridPos.x, gridPos.y) : 
            null;
    }

    private void UpdateInteractablePreview()
    {
        transform.position += new Vector3(-34, +34, 0);
        if(!_previewActive || _tileUnderPicker is null) return;
        
        _interactablePreview.transform.position = _tileUnderPicker.position + new Vector3(0.5f, 0.5f, 0);
        _previewRenderers.ForEach(r =>
        {
            r.color = ((IPlaceable)Item).AllowUse(tile: _tileUnderPicker) ? previewAllowColor : previewDenyColor;
        });
    }

    private void UpdateConsumablePreview()
    {
        if (((IConsumable) Item).AllowUse())
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
        if (((IUsableOnTile) Item).AllowUse(tile: _tileUnderPicker) && 
            !CursorManager.IsOverUI)
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
    private void ONHotBarSelectedSlotChanged(ItemSlot slot)
    {
        // Очищает пикер от предыдущего предмета
        if (itemSlot.HasItem) Clear();
        UpdatePreview(slot);
    }

    // Засовывает в пикер предмет из слота и обновляет его превью в мире
    public void UpdatePreview(ItemSlot slot)
    {
        if (!slot.HasItem) return;
        bool canPlace = slot.storedItem is IPlaceable;
        bool canUse = slot.storedItem is IUsableOnTile;
        bool canConsume = slot.storedItem is IConsumable;
        
        // Если в слоте хотбара есть предмет, и его можно использовать или поставить
        if (canConsume || canPlace || canUse)
        {
            SetItem(slot, slot.storedAmount);
            if (CursorManager.Instance.Mode == CursorMode.InWorld)
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
            // _pickedFrom.AddItem(itemSlot.storedItem, itemSlot.storedAmount);
            Clear();
        }
        // Затем обновляет превью предмета из хотбара
        // Использует инвок потому что до конца кадра инвентарь не будет закрыт
        Invoke(nameof(ToInvoke), 0.11f);
    }
    
    private void ToInvoke() => UpdatePreview(Hotbar.Instance.currentSelectedSlot);
    
}
*/
