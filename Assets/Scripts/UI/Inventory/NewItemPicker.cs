using System;
using UnityEngine;

public class NewItemPicker : MonoBehaviour
{
    #region Singleton

    public static NewItemPicker Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public InteractionDataProvider interactionDataProvider;
    public GameObject itemSlotGO;
    public InteractionBar interactionBar;
    
    [SerializeField]
    private ItemSlot itemSlot;

    private static InventoryWindow _inventoryWindow;


    public bool UseAllowed { get; private set; }
    public bool HideWhileInteracting { set; get; }

    private void Start()
    {
        _inventoryWindow = WindowManager
            .Get<InventoryWindow>(WindowIdentifier.Inventory);
        SubEvents();
    }

    private void OnDestroy()
    {
        UnsubEvents();
    }

    private void Update()
    {
        UpdatePosition();
        UpdateVisibility();
    }

    private void OnEnable()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.position = Input.mousePosition;
    }

    private void UpdateVisibility()
    {
        if (itemSlot.storedItem is not IUsable usable) return;
        if (!usable.AllowUse(interactionDataProvider.Data.Entity, interactionDataProvider.Data.Tile, interactionDataProvider.Data.Interactable))
        {
            itemSlotGO.SetActive(false);
            UseAllowed = false;
        }
        else
        { 
            itemSlotGO.SetActive(!HideWhileInteracting);
            bool inDistance = usable.IsInDistance(interactionDataProvider.Data.Entity, interactionDataProvider.Data.Tile, interactionDataProvider.Data.Interactable);
            UseAllowed = inDistance;
            FadeVisibility(!inDistance);
        }
    }

    private void FadeVisibility(bool isFaded)
    {
        itemSlot.itemIcon.color = isFaded ? 
            new Color(1f, 1f, 1f, 0.5f) : Color.white;
    }

    public void UseItem()
    {
        if(!UseAllowed) return;

        float useTime = 0.0f;

        if (itemSlot.storedItem is Instrument instrument) 
            useTime = instrument.Data.useTime;

        Interact(useTime, 
            () => ((IUsable) itemSlot.storedItem).
                Use(
                    WindowManager.Get<HotbarWindow>(WindowIdentifier.Hotbar).currentSelectedSlot.ReferredSlot,
                    interactionDataProvider.Data.Entity, 
                    interactionDataProvider.Data.Tile, 
                    interactionDataProvider.Data.Interactable), false);
    }

    public void UseHand()
    {
        Interactable interactableUnderCursor = interactionDataProvider.Data.Interactable;
        if(interactableUnderCursor is null) return;
        
        if(Vector2.Distance(PlayerController.Instance.transform.position,
            interactableUnderCursor.transform.position) > 1.6f) return;
        
        Interact(interactableUnderCursor.Data.interactingTime, () =>
            interactableUnderCursor.Interact(), true);
    }

    private void Interact(float useTime, Action action, bool isHand)
    {
        PlayerController.Instance.LookDirectionToMouse();
        PlayerController.Instance.UpdateLookDirection();
        interactionBar.StartInteraction(useTime, action, isHand);
    }
    
    private void OnSelectedHotbarSlotChanged(ItemSlot slot)
    {
        if(_inventoryWindow.IsActive) return;
        // Если в выбранном слоте хотбара есть предмет
        if (slot.HasItem && slot.storedItem is IUsable)
        {
            SyncWithSlot(slot);
            UpdatePosition();
            UpdateVisibility();
            gameObject.SetActive(true);
            return;
        }
        gameObject.SetActive(false);
    }

    public void SyncWithSlot(ItemSlot slot)
    {
        if(!slot.HasItem) gameObject.SetActive(false);
        itemSlot.storedItem = slot.storedItem;
        itemSlot.storedAmount = slot.storedAmount;
        itemSlot.UpdateUI();
    }
    
    
    private void OnWindowClosed(WindowIdentifier window)
    {
        if(window == WindowIdentifier.Inventory)
            OnSelectedHotbarSlotChanged(
                WindowManager.Get<HotbarWindow>(WindowIdentifier.Hotbar)
                    .currentSelectedSlot);
    }
    private void OnWindowOpened(WindowIdentifier window)
    {
        if(window == WindowIdentifier.Inventory)
            gameObject.SetActive(false);
    }

    private void OnCursorEnterUI()
    {
        if(_inventoryWindow.IsActive) return;
        gameObject.SetActive(false);
    }

    private void OnCursorLeaveUI()
    {
        if (_inventoryWindow.IsActive) return;
        gameObject.SetActive(true);
    }

    private void SubEvents()
    {
        HotbarWindow.ONSelectedSlotChanged += OnSelectedHotbarSlotChanged;
        BaseWindow.ONWindowOpened += OnWindowOpened;
        BaseWindow.ONWindowClosed += OnWindowClosed;
        CursorHoverCheck.ONCursorEnterUI += OnCursorEnterUI;
        CursorHoverCheck.ONCursorLeaveUI += OnCursorLeaveUI;
    }

    private void UnsubEvents()
    {
        HotbarWindow.ONSelectedSlotChanged -= OnSelectedHotbarSlotChanged;
        BaseWindow.ONWindowOpened -= OnWindowOpened;
        BaseWindow.ONWindowClosed -= OnWindowClosed;
        CursorHoverCheck.ONCursorEnterUI -= OnCursorEnterUI;
        CursorHoverCheck.ONCursorLeaveUI -= OnCursorLeaveUI;
    }
}
