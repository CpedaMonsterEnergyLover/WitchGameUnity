using System.Collections.Generic;
using UnityEngine;

public class ItemPicker : MonoBehaviour
{
    #region Singleton

    public static ItemPicker Instance;

    private void Awake()
    {
        if (Instance is null) Instance = this;
        else Debug.LogError("Found multiple instances of itempicker");
        gameObject.SetActive(false);
    }

    #endregion


    public Camera playerCamera;
    public Color previewDenyColor;
    public Color previewAllowColor;
    public InventorySlot itemSlot;
    public Item Item => itemSlot.storedItem;
    
    private GameObject _interactablePreview;
    public bool _previewActive;
    private readonly List<SpriteRenderer> _previewRenderers = new();
    
    public bool IsPlaceable => Item is IPlaceable;

    private void Start()
    {
        CursorHoverCheck.ONCursorEnterUI += ONCursorEnterUI;
        CursorHoverCheck.ONCursorLeaveUI += ONCursorLeaveUI;
    }

    public int SetItem(Item item, int count)
    {
        gameObject.SetActive(true);
        int added = itemSlot.AddItem(item, count);
        Tooltip.Instance.SetEnabled(false);

        if (IsPlaceable)
        {
            _interactablePreview = 
                Instantiate(Interactable.GetPrefab(Item.Data.placedObject.identifier));
            _interactablePreview.SetActive(false);
            CachePreviewSpriteRenderers();
        }

        return added;
    }

    public void Clear()
    {
        gameObject.SetActive(false);
        itemSlot.Clear();
        Tooltip.Instance.SetEnabled(true);
        if (_interactablePreview is not null)
        {
            DestroyImmediate(_interactablePreview);
        }
        _previewRenderers.Clear();
        _previewActive = false;
    }
    
    private void OnEnable()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 5;
        mousePosition.x -= 34;
        mousePosition.y += 34;
        transform.position = mousePosition;
        if(_previewActive) UpdatePreviewPosition();
    }
    
    private void Update()
    {
        UpdatePosition();
        CursorManager.Instance.Mode = CursorMode.HoldItem;
    }

    private void UpdatePreviewPosition()
    {
        // Обновляет позицию в соответствии с гридом
        Vector3 mouseWorldPos = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        Vector3Int gridPos = Vector3Int.FloorToInt(mouseWorldPos);

        // Меняет цвет превью в соотв. с условием
        if (WorldManager.CoordsBelongsToWorld(gridPos.x, gridPos.y))
        {
            WorldTile tile = WorldManager.WorldData.GetTile(gridPos.x, gridPos.y);
            if (tile is not null)
            {
                _interactablePreview.transform.position = gridPos + new Vector3(0.5f, 0.5f, 0);
                
                _previewRenderers.ForEach(r =>
                {
                    r.color = ((IPlaceable)Item).PlaceAllowed(tile) ? previewAllowColor : previewDenyColor;
                });
            }
        }
    }

    private void CachePreviewSpriteRenderers()
    {
        if (_interactablePreview is null) return;
        foreach (SpriteRenderer spriteRenderer in _interactablePreview.GetComponentsInChildren<SpriteRenderer>())
        {
            _previewRenderers.Add(spriteRenderer);
        }
    }

    private void ONCursorLeaveUI()
    {
        if (IsPlaceable && _interactablePreview is not null)
        {
            _interactablePreview.SetActive(true);
            itemSlot.itemIcon.enabled = false;
            // itemSlot.itemText.enabled = false;
            _previewActive = true;
        }
    }

    private void ONCursorEnterUI()
    {
        if (IsPlaceable && _interactablePreview is not null)
        {
            _interactablePreview.SetActive(false);
            itemSlot.itemIcon.enabled = true;
            // itemSlot.itemText.enabled = true;
            _previewActive = false;
        }
    }

}
