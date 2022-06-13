using System;
using UnityEngine;

public class InteractionDataProvider : MonoBehaviour
{
    private static Camera _playerCamera;
    private static int _layerMask;
    private static HotbarWindow _hotbarWindow;


    public static InteractionEventData Data { get; private set;  }

    private void Start()
    {
        _playerCamera = CameraController.camera;
        _layerMask = LayerMask.GetMask("InteractionCollider");
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
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 100, _layerMask);
        if (hit.collider is not null)
        {
            interactable = hit.collider.gameObject.GetComponent<Interactable>();
            entity = hit.collider.gameObject.GetComponent<Entity>();
        }

        // Gets tile under cursor
        Vector3Int gridPos = Vector3Int.FloorToInt(mouseWorldPos);
        WorldTile tile = WorldManager.Instance.WorldData?.GetTile(gridPos.x, gridPos.y);

        // Debug.Log($"entity: {entity?.name}, interactable: {interactable?.name}, tile: {tile?.Position}");

        
        return new InteractionEventData(tile, interactable, entity);
    }
}

[Serializable]
public readonly struct InteractionEventData
{
    public readonly WorldTile tile;
    public readonly Interactable interactable;
    public readonly Entity entity;

    public InteractionEventData(WorldTile tile, Interactable interactable, Entity entity) : this()
    {
        this.entity = entity;
        this.tile = tile;
        this.interactable = interactable;
    }

    public override string ToString()
    {
        return $"Tile: {tile?.Position}, Interactable: {interactable?.name}, Entity: {entity?.name}";
    }

    public bool Equals(InteractionEventData data)
    {
        return entity == data.entity && tile == data.tile && interactable == data.interactable;
    }
}