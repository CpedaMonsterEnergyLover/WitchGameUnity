using UnityEngine;

public class MagicBook : Item, IUsable, IHasOwnInteractionTime, IUsableOnAnyTarget, IControlsUsabilityInMove, IEventOnUseStart, IParticleEmitterItem
{
    public new MagicBookData Data => (MagicBookData) data;
    
    private bool _inCooldown;

    public float InteractionTime => Data.castTime;
    public bool CanUseMoving => !Data.canCastInMove;

    public void Use(ItemSlot slot, Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        if(!AllowUse(entity, tile, interactable)) return;
        var playerPos = PlayerManager.Instance.Position3;
        var position = playerPos + 
                       (playerPos - CameraController.camera.ScreenToWorldPoint(Input.mousePosition)).normalized * -0.3f;
        BulletSpawner.SingleBullet(Data.bullet, position);
    }

    public bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null) 
        => !_inCooldown;

    public bool IsInDistance(Entity entity = null, WorldTile tile = null, Interactable interactable = null) => true;
    
    public MagicBook(ItemIdentifier identifier) : base(identifier)
    {
    }

    public void OnUseStart()
    {
        if(!Data.hasParticles) return;
        ToolHolder.Instance.StartAnimation(
            new ToolSwipeAnimationData(
                ToolSwipeAnimationType.Swipe,
                1f / InteractionTime, 
                Data.cooldown,
                Data.autoShoot, 
                true));
    }

    public bool HasParticles => Data.hasParticles;
    public ParticleSystem ParticleSystem => Data.particles;
    public ItemParticleEmissionMode EmissionMode => ItemParticleEmissionMode.EmitOnUse;
}