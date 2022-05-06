using UnityEngine;

public class Food : Item, IConsumable
{

    public new FoodData Data => (FoodData) data;

    public Food(ItemIdentifier identifier) : base(identifier)
    {
    }

    public void Use(ItemSlot slot, Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        slot.RemoveItem(1);
        Debug.Log($"{Data.name} consumed! {Data.saturation} hunger restored");
    }

    public bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null) => true;
    
    public bool IsInDistance(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        if (entity is null) return true;
        Vector2 playerPos = PlayerManager.Instance.Position;
        return Vector2.Distance(playerPos, entity.transform.position) <= 2 ; 
    }
}
