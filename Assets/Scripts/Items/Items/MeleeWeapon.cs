public class MeleeWeapon : Item, IUsable, IUsableOnAnyTarget, IToolHolderFullSprite, IControlsUsabilityInMove
{
    public new MeleeWeaponData Data => (MeleeWeaponData) data;
    
    protected override string GetDescription()
    {
        return base.GetDescription() + $"\nСкорость атаки: {Data.speed}" ;
    }

    public void Use(ItemSlot slot, Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        if(!AllowUse()) return;
        ToolHolder.Instance.Attack(this);
    }

    public bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
        => !ToolHolder.Instance.InUse;

    public bool IsInDistance(Entity entity = null, WorldTile tile = null, Interactable interactable = null) => true;

    public MeleeWeapon(ItemIdentifier identifier) : base(identifier)
    {
    }

    public bool CanUseMoving => true;
}
