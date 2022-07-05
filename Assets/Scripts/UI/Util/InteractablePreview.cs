using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractablePreview : MonoBehaviour
{
    private PlaceableItem _currentItem;
    private GameObject _preview;

    private static readonly Color CanPlaceColor = new Color(0.49f, 1f, 0.46f, 0.7f);
    private static readonly Color CannotPlaceColor = new Color(1f, 0.25f, 0.27f, 0.7f);
    private static readonly Color OutOfRangeColor = new Color(1f, 1f, 1f, 0.4f);
    private List<SpriteRenderer> _spriteRenderers;

    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>().ToList();
    }

    private void Update()
    {
        if(InteractionDataProvider.Data.tile is null) return;
        UpdatePreview();
    }

    public void Show(PlaceableItem placeableItem)
    {
        _currentItem = placeableItem;
        UpdatePreview();
        gameObject.SetActive(true);
    }

    private void UpdatePreview()
    {
        UpdatePreviewPosition();
        UpdatePreviewColor();
    }

    private void UpdatePreviewPosition()
    {
        WorldTile tile = InteractionDataProvider.Data.tile;
        if(tile is null) return;
        Vector2Int tilePos = tile.Position;
        Vector3 newPos = new Vector3(
            tilePos.x + 0.5f, tilePos.y + 0.5f, 0);
        transform.position = newPos;
    }

    private void UpdatePreviewColor()
    {
        InteractionEventData eventData = InteractionDataProvider.Data;
        Color finalColor = _currentItem.IsInDistance(null, eventData.tile) ? 
            _currentItem.AllowUse(eventData.entity, eventData.tile, eventData.interactable) ? 
                CanPlaceColor : CannotPlaceColor
            : OutOfRangeColor;
        
        foreach (SpriteRenderer renderer in _spriteRenderers)
        {
            renderer.color = finalColor;
        }
    }

}
