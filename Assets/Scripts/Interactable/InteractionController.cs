using System;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public Camera playerCamera;
    public NewItemPicker newItemPicker;

    public InteractionControllerData Data { get; private set;  }

    private void Update()
    {
        Data = ForceUpdateData();
        ListenToKeyboard();
    }

    private void ListenToKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (newItemPicker.gameObject.activeInHierarchy)
                if (newItemPicker.itemSlotGO.activeInHierarchy)
                    newItemPicker.UseItem();
                else
                    newItemPicker.UseHand();
            else
                newItemPicker.UseHand();
        }
    }


    public InteractionControllerData ForceUpdateData()
    {
        Vector3 mouseWorldPos = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Entity entity = null;
        Interactable interactable = null;
        
        // Gets interactable and entity under cursor
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
        if (hit.collider is not null)
        {
            interactable = hit.collider.gameObject.GetComponent<Interactable>();
            entity = hit.collider.gameObject.GetComponent<Entity>();
        }

        // Gets tile under cursor
        Vector3Int gridPos = Vector3Int.FloorToInt(mouseWorldPos);
        WorldTile tile = WorldManager.Instance.CoordsBelongsToWorld(gridPos.x, gridPos.y) ? 
            WorldManager.Instance.WorldData.GetTile(gridPos.x, gridPos.y) : 
            null;

        return new InteractionControllerData(tile, interactable, entity);
    }
}

[Serializable]
public readonly struct InteractionControllerData
{
    public readonly WorldTile Tile;
    public readonly Interactable Interactable;
    public readonly Entity Entity;

    public InteractionControllerData(WorldTile tile, Interactable interactable, Entity entity) : this()
    {
        Entity = entity;
        Tile = tile;
        Interactable = interactable;
    }
}