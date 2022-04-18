using System.Threading.Tasks;
using UnityEngine;

public class MagicBook : Item, IUsable
{
    public new MagicBookData Data => (MagicBookData) data;
    
    private bool _inCooldown;

    public void Use(ItemSlot slot, Entity entity = null, WorldTile tile = null, Interactable interactable = null)
    {
        _inCooldown = true;
        var position = PlayerManager.Instance.Position + 
                       (PlayerManager.Instance.Position - CameraController.camera.ScreenToWorldPoint(Input.mousePosition)).normalized * -0.3f;
        BulletSpawner.SingleBullet(Data.bullet, position);
        ResetCooldown().GetAwaiter();
    }

    public bool AllowUse(Entity entity = null, WorldTile tile = null, Interactable interactable = null) 
        => !_inCooldown;

    public bool IsInDistance(Entity entity = null, WorldTile tile = null, Interactable interactable = null) => true;
    
    public MagicBook(ItemIdentifier identifier) : base(identifier)
    {
    }

    private async Task ResetCooldown()
    {
        await Task.Delay(Data.cooldownMS);
        _inCooldown = false;
    }
}