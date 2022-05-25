using UnityEngine;
using UnityEngine.UI;

//TODO: Make preview GO, not image
public class InteractablePreview : MonoBehaviour
{
    public Image preview;
    
    private PlaceableItem _currentItem;
    private PlaceableData Data => _currentItem?.Data;


    private static readonly Color CanPlaceColor = new Color(0.49f, 1f, 0.46f, 0.7f);
    private static readonly Color CannotPlaceColor = new Color(1f, 0.25f, 0.27f, 0.7f);
    private static readonly Color OutOfRangeColor = new Color(1f, 1f, 1f, 0.4f);
    
    private void Update()
    {
        if(InteractionDataProvider.Data.tile is null) return;
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
        preview.color = finalColor;
    }

}
