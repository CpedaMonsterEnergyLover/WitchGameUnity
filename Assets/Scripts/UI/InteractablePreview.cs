using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractablePreview : MonoBehaviour
{
    public Image preview;
    private PlaceableItem _currentItem;
    private PlaceableData Data => _currentItem?.Data;

    public static InteractablePreview Instance;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
        preview.enabled = true;
    }

    private void Update()
    {
        if(InteractionDataProvider.Data.Tile is null) return;
        UpdatePreview();
    }

    public void Show(PlaceableItem item)
    {
        if (item.Data != Data)
        {
            _currentItem = item;
            preview.sprite = Data.preview;
        }
        preview.SetNativeSize();
        UpdatePreview();
        gameObject.SetActive(true);
    }

    private void UpdatePreview()
    {
        UpdatePreviewPosition();
        UpdatePreviewColor();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void UpdatePreviewPosition()
    {
        Vector3 newPos = (Vector2)InteractionDataProvider.Data.Tile.Position;
        newPos += new Vector3(0.5f, 0.5f, 0);
        newPos.z = 0;
        transform.position = newPos;
    }

    private void UpdatePreviewColor()
    {
        InteractionEventData eventData = InteractionDataProvider.Data;
        Color finalColor = _currentItem.IsInDistance(null, eventData.Tile, null) ? 
            _currentItem.AllowUse(eventData.Entity, eventData.Tile, eventData.Interactable) ? 
                new Color(0.49f, 1f, 0.46f, 0.7f) : 
                new Color(1f, 0.25f, 0.27f, 0.7f)
            : new Color(1f, 1f, 1f, 0.4f);
            preview.color = finalColor;
    }

}
