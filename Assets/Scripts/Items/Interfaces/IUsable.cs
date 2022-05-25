using System;
using UnityEngine;

public interface IUsable
{
    public void Use(ItemSlot slot, Entity entity = null, WorldTile tile = null, Interactable interactable = null) { }

    public bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null) 
        => false;

    public bool IsInDistance(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        Vector3 targetPosition = Vector3.negativeInfinity;
        if (entity is not null)
        {
            targetPosition = entity.transform.position;
        } else if (interactable is not null)
        {
            targetPosition = interactable.transform.position;
        } else if (tile is not null)
        {
            targetPosition = new Vector3(tile.Position.x + 0.5f, tile.Position.y + 0.5f, 0);
        }

        return Vector2.Distance(PlayerManager.Instance.Pos2D, targetPosition) <= 1.25f;
    }
}