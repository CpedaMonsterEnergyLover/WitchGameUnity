using UnityEngine;

public class MeleeWeapon : Item, IUsable, IUsableOnAnyTarget, IHoldAsTool, IControlsUsabilityInMove, IParticleEmitterItem, IControlsInteractionContinuation
{
    public new MeleeWeaponData Data => (MeleeWeaponData) data;
    
    protected override string GetDescription()
    {
        return base.GetDescription() + $"\nСкорость атаки: {Data.speed}" ;
    }

    public void Use(ItemSlot slot, Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        if(!AllowUse()) return;

        /*ToolSwipeAnimationData animationData = new ToolSwipeAnimationData(
            ToolSwipeAnimationType.Swipe,
            Data.speed,
            Data.cooldown,
            Data.allowSpam,
            false);
        ToolHolder.Instance.StartAnimation(animationData);*/
    }

    public bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null)
        => !ToolHolder.Instance.InUse;

    public bool IsInDistance(Entity entity = null, WorldTile tile = null, Interactable interactable = null) => true;

    public MeleeWeapon(ItemIdentifier identifier) : base(identifier)
    {
    }

    public bool CanUseMoving => true;
    public bool HasParticles => Data.hasParticles;
    public ParticleSystem ParticleSystem => Data.particleSystem;
    public ItemParticleEmissionMode EmissionMode => ItemParticleEmissionMode.AlwaysEmit;
    public bool AllowContinuation => Data.allowSpam;
}
