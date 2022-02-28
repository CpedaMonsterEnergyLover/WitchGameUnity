using UnityEngine;

public class NewItemPicker : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject itemSlotGO;
    
    [SerializeField]
    private ItemSlot itemSlot;

    private WorldTile _tileUnderCursor;
    private Interactable _interactableUnderCursor;
    private Entity _entityUnderCursor;
    private bool _useAllowed;

    private void Start()
    {
        SubEvents();
    }

    private void OnDestroy()
    {
        UnsubEvents();
    }

    private void Update()
    {
        GetObjectsUnderCursor();
        UpdatePosition();
        UpdateVisibility();
        ListenToKeyboard();
    }

    private void OnEnable()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 5;
        transform.position = mousePosition;// + new Vector3(-34, +34, 0);
    }

    private void UpdateVisibility()
    {
        if (itemSlot.storedItem is not IUsable usable) return;
        if (!usable.AllowUse(_entityUnderCursor, _tileUnderCursor, _interactableUnderCursor))
        {
            itemSlotGO.SetActive(false);
            _useAllowed = false;
        }
        else
        {
            itemSlotGO.SetActive(true);
            bool inDistance = usable.IsInDistance(_entityUnderCursor, _tileUnderCursor, _interactableUnderCursor);
            _useAllowed = inDistance;
            FadeVisibility(!inDistance);
        }
    }

    private void FadeVisibility(bool isFaded)
    {
        itemSlot.itemIcon.color = isFaded ? 
            new Color(1f, 1f, 1f, 0.5f) : Color.white;
    }

    private void ListenToKeyboard()
    {
        if(!_useAllowed) return;
        if(Input.GetKeyDown(KeyCode.Mouse0) || Input.GetMouseButton(0)) ((IUsable)itemSlot.storedItem).Use(
            _entityUnderCursor, _tileUnderCursor, _interactableUnderCursor);
    }
    
    private void OnSelectedHotbarSlotChanged(ItemSlot slot)
    {
        // TODO: IUsableOnEntity, IUsableOnInteractable, IUsableOnTile
        
        if(Inventory.Instance.IsActive) return;
        // Если в выбранном слоте хотбара есть предмет
        if (slot.HasItem)
        {
            if (slot.storedItem is IUsable)
            {
                itemSlot.storedItem = slot.storedItem;
                itemSlot.storedAmount = slot.storedAmount;
                itemSlot.UpdateUI();
                GetObjectsUnderCursor();
                UpdatePosition();
                UpdateVisibility();
                gameObject.SetActive(true);
            } else {
                gameObject.SetActive(false);
            }
        }
        // Если нет предмета
        else
        {
            gameObject.SetActive(false);
        }

    }

    private void GetObjectsUnderCursor()
    {
        Vector3 mouseWorldPos = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        // Gets interactable and entity under cursor
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
        if (hit.collider is not null)
        {
            _interactableUnderCursor = hit.collider.gameObject.GetComponent<Interactable>();
            _entityUnderCursor = hit.collider.gameObject.GetComponent<Entity>();
        }
        else
        {
            _interactableUnderCursor = null;
            _entityUnderCursor = null;
        }

        // Gets tile under cursor
        Vector3Int gridPos = Vector3Int.FloorToInt(mouseWorldPos);
        _tileUnderCursor = WorldManager.Instance.CoordsBelongsToWorld(gridPos.x, gridPos.y) ? 
            WorldManager.Instance.WorldData.GetTile(gridPos.x, gridPos.y) : 
            null;
    }
    
    private void OnInventoryClosed()
    {
        OnSelectedHotbarSlotChanged(Hotbar.Instance.currentSelectedSlot);
    }

    private void OnInventoryOpened()
    {
        gameObject.SetActive(false);
    }

    private void SubEvents()
    {
        Hotbar.ONSelectedSlotChanged += OnSelectedHotbarSlotChanged;
        Inventory.ONInventoryOpened += OnInventoryOpened;
        Inventory.ONInventoryClosed += OnInventoryClosed;
    }

    private void UnsubEvents()
    {
        Hotbar.ONSelectedSlotChanged -= OnSelectedHotbarSlotChanged;
        Inventory.ONInventoryOpened -= OnInventoryOpened;
        Inventory.ONInventoryClosed -= OnInventoryClosed;
    }
}
