using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceableWindow : BaseWindow
{
    public List<Component> toDismiss = new();
    public Transform slotTransform;
    public Text titleText;

    private TemporaryDismissData _dismissData;
    private Transform _slotParent;
    private ItemSlot _slot;
    private PlaceableItem _placeableItem;

    private InteractablePreview _previewInstance;
    
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
        
        InstantiatePreview(placeableItem);
        SetActive(true);
    }

    protected override void OnDisable()
    {
        _dismissData = _dismissData.ShowAll();
        _slot.transform.SetParent(_slotParent, false);
        if (_slot is CraftingSlot) _slot.storedItem = null;
        _slot = null;
        _slotParent = null;
        
        RemovePreview();
        base.OnDisable();
    }

    private void InstantiatePreview(PlaceableItem placeableItem)
    {
        RemovePreview();
        _previewInstance = Instantiate(placeableItem.Data.preview)
            .AddComponent<InteractablePreview>();
        _previewInstance.Show(placeableItem);
    }

    private void RemovePreview()
    {
        if (_previewInstance is not null)
        {
            Destroy(_previewInstance.gameObject);
            _previewInstance = null;
        }
    }

    public void Place()
    {
        var data = InteractionDataProvider.Data;
        if (!_placeableItem.AllowUse(data.entity, data.tile, data.interactable)) return;
        
        _placeableItem.Use(_slot, null, data.tile);

        if (_slot is CraftingSlot craftingSlot)
        {
            craftingSlot.ConsumeRecipeItems(1);
        }
        
        _slot.UpdateUI();
        if(_slot.storedAmount <= 0) gameObject.SetActive(false);
    }

}
