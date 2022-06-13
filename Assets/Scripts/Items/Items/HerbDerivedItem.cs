using UnityEngine;

public class HerbDerivedItem : Item, IConsumable, IHasOwnInteractionTime, IControlsInteractionContinuation
{
    public new HerbDerivedItemData Data => (HerbDerivedItemData) data;
    
    public HerbDerivedItem(ItemIdentifier identifier) : base(identifier)
    {
    }
    
    public void Use(ItemSlot slot, Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        slot.RemoveItem(1);
        PlayerManager.Instance.AddHunger(5);
        if(Data.hasEffectOnEat) PlayerManager.Instance.ApplyEffect(Data.heartEffect);
    }

    public bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null) => 
        PlayerManager.Instance.CanEat;

    public bool IsInDistance(Entity entity = null, WorldTile tile = null, Interactable interactable = null) =>
        PlayerManager.Instance.CanEat;

    public float InteractionTime => 1f;
    public bool AllowContinuation => false;
}