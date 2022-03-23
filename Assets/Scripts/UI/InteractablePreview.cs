using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePreview : MonoBehaviour
{
    private GameObject _preview;
    private PlaceableItem _currentItem;
    private PlaceableData Data => _currentItem?.Data;
    private SpriteRenderer[] _cachedRenderers;

    public static InteractablePreview Instance;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
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
            if(_preview is not null)
                Destroy(_preview);
            _currentItem = item;
            _preview = Instantiate(Data.prefab);
            CacheRenderers();
            _preview.name = "InteractablePreview";
        }
        UpdatePreview();
        _preview.SetActive(true);
        gameObject.SetActive(true);
    }

    private void UpdatePreview()
    {
        UpdatePreviewPosition();
        UpdatePreviewColor();
    }

    public void Hide()
    {
        _preview.SetActive(false);
        gameObject.SetActive(false);
    }

    private void UpdatePreviewPosition()
    {
        _preview.transform.position =
            InteractionDataProvider.Data.Tile.Position + new Vector2(0.5f, 0.5f);
    }

    private void UpdatePreviewColor()
    {
        InteractionEventData eventData = InteractionDataProvider.Data;
        Color finalColor = _currentItem.IsInDistance(null, eventData.Tile, null) ? 
            _currentItem.AllowUse(eventData.Entity, eventData.Tile, eventData.Interactable) ? 
                new Color(0.49f, 1f, 0.46f, 0.7f) : new Color(1f, 0.25f, 0.27f, 0.7f)
            : new Color(1f, 1f, 1f, 0.4f);
        foreach (SpriteRenderer spriteRenderer in _cachedRenderers)
        {
            spriteRenderer.color = finalColor;
        }
    }

    private void CacheRenderers()
    {
        _cachedRenderers = _preview.GetComponentsInChildren<SpriteRenderer>();
    }
}
