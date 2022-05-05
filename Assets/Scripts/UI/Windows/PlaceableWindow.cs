using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceableWindow : BaseWindow
{
    public List<Component> toDismiss = new();
    public Transform slotTransform;
    public Text titleText;
    public InteractablePreview interactablePreview;

    private GameObject _instantiatedPrefab;

    private TemporaryDismissData _dismissData;
    private Transform _slotParent;
    private ItemSlot _slot;
    private PlaceableItem _placeableItem;

    public void Show(ItemSlot slot)
    {
        if(slot is CraftingSlot craftingSlot)
            slot.storedItem = Item.Create(craftingSlot.recipe.result.item.identifier);

        if (slot.storedItem is not PlaceableItem placeableItem)
        {
            gameObject.SetActive(false);
            _placeableItem = null;
            return;
        }
        
        // EnablePreview
        

        _placeableItem = placeableItem;
        
        _dismissData = new TemporaryDismissData()
            .Add(toDismiss).HideAll();
        _slot = slot;
        var currentSlotTransform = slot.transform;
        // Save previous parent
        _slotParent = currentSlotTransform.parent;
        // Set new parent
        currentSlotTransform.SetParent(slotTransform, false);
        currentSlotTransform.localPosition = Vector3.zero;

        titleText.text = _slot.storedItem.Data.name;
        
        interactablePreview.Show(_placeableItem);
        
        SetActive(true);
    }

    protected override void OnDisable()
    {
        // Disable preview
        _dismissData = _dismissData.ShowAll();
        _slot.transform.SetParent(_slotParent, false);
        if (_slot is CraftingSlot) _slot.storedItem = null;
        _slot = null;
        _slotParent = null;
        
        interactablePreview.Hide();
        base.OnDisable();
    }

    public void Place()
    {
        
        var data = InteractionDataProvider.Data;
        if (!_placeableItem.AllowUse(data.Entity, data.Tile, data.Interactable)) return;
        
        _placeableItem.Use(_slot, null, data.Tile);

        if (_slot is CraftingSlot craftingSlot)
        {
            craftingSlot.ConsumeRecipeItems(1);
        }
        
        _slot.UpdateUI();
        // Close window if item is over
        if(_slot.storedAmount <= 0) gameObject.SetActive(false);
    }

}
