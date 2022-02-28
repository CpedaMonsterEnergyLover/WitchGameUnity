using UnityEngine;

public class Food : Item, IConsumable
{

    public new FoodData Data => (FoodData) data;

    public Food(ItemIdentifier identifier) : base(identifier)
    {
    }

    public void Use(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        Debug.Log($"{Data.name} consumed! {Data.saturation} hunger restored");
    }

    public bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null) => true;
    
    public bool IsInDistance(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        Vector2 playerPos = WorldManager.Instance.playerTransform.position;
        if (entity is null) return true;
        return Vector2.Distance(playerPos, entity.transform.position) <= 2 ; 
    }
}
