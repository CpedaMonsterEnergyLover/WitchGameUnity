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

    public InteractionController interactionController;
    public GameObject itemSlotGO;
    public InteractionBar interactionBar;
    
    [SerializeField]
    private ItemSlot itemSlot;


    public bool UseAllowed { get; private set; }
    public bool HideWhileInteracting { set; get; }

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
        if (!usable.AllowUse(interactionController.Data.Entity, interactionController.Data.Tile, interactionController.Data.Interactable))
        {
            itemSlotGO.SetActive(false);
            UseAllowed = false;
        }
        else
        { 
            itemSlotGO.SetActive(!HideWhileInteracting);
            bool inDistance = usable.IsInDistance(interactionController.Data.Entity, interactionController.Data.Tile, interactionController.Data.Interactable);
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
        {
            if (instrument.InstanceData.Durability <= 0)
            {
                itemSlot.Shake();
                return;
            }
            useTime = instrument.Data.useTime;
        }
        
        Interact(useTime, 
            () => ((IUsable) itemSlot.storedItem).
                Use(
                    Hotbar.Instance.currentSelectedSlot.ReferredSlot,
                    interactionController.Data.Entity, 
                    interactionController.Data.Tile, 
                    interactionController.Data.Interactable));
    }

    public void UseHand()
    {
        Interactable interactableUnderCursor = interactionController.Data.Interactable;
        if(interactableUnderCursor is null) return;
        
        if(Vector2.Distance(PlayerController.Instance.transform.position,
            interactableUnderCursor.transform.position) > 1.6f) return;
        
        Interact(1f, () =>
            interactableUnderCursor.Interact());
    }

    private void Interact(float useTime, Action action)
    {
        PlayerController.Instance.LookDirectionToMouse();
        PlayerController.Instance.UpdateLookDirection();
        interactionBar.StartInteraction(useTime, action);
    }
    
    private void OnSelectedHotbarSlotChanged(ItemSlot slot)
    {
        if(Inventory.Instance.IsActive) return;
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
    
    
    private void OnInventoryClosed()
    {
        OnSelectedHotbarSlotChanged(Hotbar.Instance.currentSelectedSlot);
    }
    private void OnInventoryOpened()
    {
        gameObject.SetActive(false);
    }

    private void OnCursorEnterUI()
    {
        if(Inventory.Instance.IsActive) return;
        gameObject.SetActive(false);
    }

    private void OnCursorLeaveUI()
    {
        if (Inventory.Instance.IsActive) return;
        gameObject.SetActive(true);
    }

    private void SubEvents()
    {
        Hotbar.ONSelectedSlotChanged += OnSelectedHotbarSlotChanged;
        Inventory.ONInventoryOpened += OnInventoryOpened;
        Inventory.ONInventoryClosed += OnInventoryClosed;
        CursorHoverCheck.ONCursorEnterUI += OnCursorEnterUI;
        CursorHoverCheck.ONCursorLeaveUI += OnCursorLeaveUI;
    }

    private void UnsubEvents()
    {
        Hotbar.ONSelectedSlotChanged -= OnSelectedHotbarSlotChanged;
        Inventory.ONInventoryOpened -= OnInventoryOpened;
        Inventory.ONInventoryClosed -= OnInventoryClosed;
        CursorHoverCheck.ONCursorEnterUI -= OnCursorEnterUI;
        CursorHoverCheck.ONCursorLeaveUI -= OnCursorLeaveUI;
    }
}
