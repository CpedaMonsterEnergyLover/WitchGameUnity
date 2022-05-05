using System;
using UnityEngine;

public interface IUsable
{
    public void Use(ItemSlot slot, Entity entity = null, WorldTile tile = null, Interactable interactable = null) { }

    public bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null) 
        => false;

    public bool IsInDistance(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        Vector2 targetPosition = Vector2.negativeInfinity;
        if (entity is not null)
        {
            targetPosition = entity.transform.position;
        } else if (interactable is not null)
        {
            targetPosition = interactable.transform.position;
        } else if (tile is not null)
        {
            targetPosition = tile.Position + new Vector2(0.5f, 0.5f);
        }

        return Vector2.Distance(WorldManager.Instance.playerTransform.position, targetPosition) <= 1.6f;
    }
}