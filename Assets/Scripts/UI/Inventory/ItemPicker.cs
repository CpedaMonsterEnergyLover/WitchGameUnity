using System;
using UnityEngine;

public class ItemPicker : MonoBehaviour, ITemporaryDismissable
{
    #region Singleton

    public static ItemPicker Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion


    public GameObject itemSlotGO;
    public InteractionBar interactionBar;
    
    [SerializeField]
    private ItemSlot itemSlot;

    private static InventoryWindow _inventoryWindow;
    private static HotbarWindow _hotbarWindow;


    public bool UseAllowed { get; private set; }
    public bool HideWhileInteracting { set; get; }


    #region ITemporaryDismissable

    public bool IsActive => gameObject.activeInHierarchy;
    public void SetActive(bool isActive) => gameObject.SetActive(isActive);

    #endregion
    
    private void Start()
    {
        _inventoryWindow = WindowManager
            .Get<InventoryWindow>(WindowIdentifier.Inventory);
        _hotbarWindow = WindowManager
            .Get<HotbarWindow>(WindowIdentifier.Hotbar);
        SubEvents();
    }

    private void OnDestroy()
    {
        UnsubEvents();
    }

    private void Update()
    {
        UpdateVisibility();
    }


    private void UpdateVisibility()
    {
        if (itemSlot.storedItem is not IUsable usable) return;
        if (!usable.AllowUse(InteractionDataProvider.Data.Entity, InteractionDataProvider.Data.Tile, InteractionDataProvider.Data.Interactable))
        {
            itemSlotGO.SetActive(false);
            UseAllowed = false;
        }
        else
        { 
            itemSlotGO.SetActive(!HideWhileInteracting);
            bool inDistance = usable.IsInDistance(InteractionDataProvider.Data.Entity, InteractionDataProvider.Data.Tile, InteractionDataProvider.Data.Interactable);
            UseAllowed = inDistance;
            FadeVisibility(!inDistance);
        }
    }

    private void FadeVisibility(bool isFaded)
    {
        itemSlot.itemIcon.color = isFaded ? 
            new Color(1f, 1f, 1f, 0.5f) : Color.white;
    }

    public void Use()
    {
        if (gameObject.activeInHierarchy)
            if (itemSlotGO.activeInHierarchy)
                UseItem();
            else
                UseHand();
        else
            UseHand();
    }

    private void UseItem()
    {
        if(!UseAllowed) return;

        Item storedItem = itemSlot.storedItem;
        float useTime = storedItem  is IHasOwnInteractionTime item ? 
            item.InteractionTime : 0.0f;
        InteractionFilter filter = new InteractionFilter(
            storedItem is IControlsUsabilityInMove controller ? controller.CanUseMoving : false, 
            storedItem is not IUsableOnAnyTarget);
        
        Interact(useTime, () => ((IUsable) itemSlot.storedItem).
                Use(WindowManager.Get<HotbarWindow>(WindowIdentifier.Hotbar).currentSelectedSlot.ReferredSlot,
                    InteractionDataProvider.Data.Entity, 
                    InteractionDataProvider.Data.Tile, 
                    InteractionDataProvider.Data.Interactable), false, filter);
    }

    private void UseHand()
    {
        Interactable interactableUnderCursor = InteractionDataProvider.Data.Interactable;
        if(interactableUnderCursor is null) return;
        
        if(Vector2.Distance(PlayerController.Instance.transform.position,
            interactableUnderCursor.transform.position) > 1.6f) return;

        InteractionFilter filter = new InteractionFilter(true, true);
        Interact(interactableUnderCursor.Data.interactingTime, 
            () => interactableUnderCursor.Interact(), true, filter);
    }

    private void Interact(float useTime, Action action, bool isHand, InteractionFilter filter)
    {
        PlayerController.Instance.LookDirectionToMouse();
        PlayerController.Instance.UpdateLookDirection();
        interactionBar.StartInteraction(useTime, action, isHand, filter);
    }
    
    private void OnSelectedHotbarSlotChanged(ItemSlot slot)
    {
        if(_inventoryWindow.IsActive) return;
        // Если в выбранном слоте хотбара есть предмет
        if (slot.HasItem && slot.storedItem is IUsable)
        {
            SyncWithSlot(slot);
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
        if(window is WindowIdentifier.Inventory or WindowIdentifier.Placeable)
            OnSelectedHotbarSlotChanged(_hotbarWindow.currentSelectedSlot);
    }
    private void OnWindowOpened(WindowIdentifier window)
    {
        if(window is WindowIdentifier.Inventory or WindowIdentifier.Placeable)
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
