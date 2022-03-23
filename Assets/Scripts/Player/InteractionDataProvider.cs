using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class InteractionDataProvider : MonoBehaviour
{
    private static Camera _playerCamera;

    public static InteractionEventData Data { get; private set;  }

    private void Awake()
    {
        _playerCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        Data = ForceUpdateData();
    }

    public static InteractionEventData ForceUpdateData()
    {
        Vector3 mouseWorldPos = _playerCamera.ScreenToWorldPoint(Input.mousePosition);
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

        return new InteractionEventData(tile, interactable, entity);
    }
}

[Serializable]
public readonly struct InteractionEventData
{
    public readonly WorldTile Tile;
    public readonly Interactable Interactable;
    public readonly Entity Entity;

    public InteractionEventData(WorldTile tile, Interactable interactable, Entity entity) : this()
    {
        Entity = entity;
        Tile = tile;
        Interactable = interactable;
    }

    public override string ToString()
    {
        return $"Tile: {Tile?.Position}, Int: {Interactable?.name}, Entity: {Entity?.name}";
    }

    public bool Equals(InteractionEventData data)
    {
        return Entity == data.Entity && Tile == data.Tile && Interactable == data.Interactable;
    }
}