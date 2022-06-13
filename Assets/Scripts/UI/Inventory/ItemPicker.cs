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
        if (!usable.AllowUse(
            InteractionDataProvider.Data.entity, 
            InteractionDataProvider.Data.tile, 
            InteractionDataProvider.Data.interactable))
        {
            itemSlotGO.SetActive(false);
            UseAllowed = false;
        }
        else
        { 
            itemSlotGO.SetActive(!HideWhileInteracting);
            UseAllowed = usable.IsInDistance(
                InteractionDataProvider.Data.entity, 
                InteractionDataProvider.Data.tile, 
                InteractionDataProvider.Data.interactable);
            FadeVisibility(!UseAllowed);
        }
    }


    private static readonly Color FadedColor = new(1f, 1f, 1f, 0.5f);
    private void FadeVisibility(bool isFaded)
    {
        itemSlot.itemIcon.color = isFaded ? 
            FadedColor : Color.white;
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
        if(itemSlot.storedItem is not IUsable storedItem) return;
        float useTime = storedItem  is IHasOwnInteractionTime item ? 
            item.InteractionTime : 0.0f;

        Action onStart = storedItem is IEventOnUseStart eventInvoker ? eventInvoker.OnUseStart : null;

        InteractionFilter filter = new InteractionFilter(
        storedItem is not IControlsUsabilityInMove moveController || moveController.CanUseMoving, 
        storedItem is not IUsableOnAnyTarget);

        bool allowContinuation = storedItem is not IControlsInteractionContinuation continuationController ||
                                 continuationController.AllowContinuation;

        ToolSwipeAnimationData animationData = storedItem is IHasToolAnimation animator ? 
            animator.AnimationData : null; 
        
        Interact(useTime, () => storedItem
            .Use(_hotbarWindow.SelectedSlot.ReferenceSlot,
                InteractionDataProvider.Data.entity, 
                InteractionDataProvider.Data.tile, 
                InteractionDataProvider.Data.interactable),
            onStart, false, allowContinuation, filter, animationData);

    }

    private void UseHand()
    {
        Interactable interactableUnderCursor = InteractionDataProvider.Data.interactable;
        if(interactableUnderCursor is null) return;
        
        if(Vector2.Distance(PlayerController.Instance.transform.position,
            interactableUnderCursor.transform.position) > 1.3f) return;

        InteractionFilter filter = new InteractionFilter(true, true);
        Interact(interactableUnderCursor.Data.interactingTime, 
            () => interactableUnderCursor.Interact(),
            null, true, true, filter, null);
    }

    private void Interact(float useTime, Action actionOnEnd, Action actionOnStart,
        bool isHand, bool allowContinuation, InteractionFilter filter,
        ToolSwipeAnimationData animationData)
    {
        PlayerController.Instance.LookDirectionToMouse();
        PlayerController.Instance.UpdateLookDirection();
        interactionBar.StartInteraction(useTime, actionOnEnd, actionOnStart, isHand,
            allowContinuation, filter, animationData);
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
            OnSelectedHotbarSlotChanged(_hotbarWindow.SelectedSlot);
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

    public void StopInteraction() => interactionBar.StopInteraction();
}
