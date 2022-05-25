using UnityEngine;

public class MagicBook : Item, IHasOwnInteractionTime, IUsableOnAnyTarget, IControlsUsabilityInMove, IParticleEmitterItem, IHasToolAnimation
{
    public new MagicBookData Data => (MagicBookData) data;
    
    private bool _inCooldown;

    public float InteractionTime => Data.castTime;
    public bool CanUseMoving => !Data.canCastInMove;

    public void Use(ItemSlot slot, Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        if(!AllowUse(entity, tile, interactable)) return;
        var playerPos = PlayerManager.Instance.Pos3D;
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
    
    public bool HasParticles => Data.hasParticles;
    public ParticleSystem ParticleSystem => Data.particles;
    public ItemParticleEmissionMode EmissionMode => ItemParticleEmissionMode.EmitOnUse;
    public ToolSwipeAnimationData AnimationData => new(
            ToolSwipeAnimationType.Swipe,
            1f / InteractionTime,
            Data.cooldown,
            Data.autoShoot,
            true);
}