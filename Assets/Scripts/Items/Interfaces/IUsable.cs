using System;

public interface IUsable
{
    public void Use(ItemSlot slot, Entity entity = null, WorldTile tile = null, Interactable interactable = null)
        => throw new NotImplementedException();
    
    public bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null) 
        => false;

    public bool IsInDistance(Entity entity = null, WorldTile tile = null, Interactable interactable = null) =>
        throw new NotImplementedException();
}