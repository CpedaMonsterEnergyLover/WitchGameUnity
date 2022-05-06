using UnityEngine;

public class MagicBook : Item, IUsable, IHasOwnInteractionTime, IUsableOnAnyTarget, IControlsUsabilityInMove
{
    public new MagicBookData Data => (MagicBookData) data;
    
    private bool _inCooldown;

    public float InteractionTime => Data.cooldown;
    public bool CanUseMoving => !Data.canCastInMove;

    public void Use(ItemSlot slot, Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
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
}