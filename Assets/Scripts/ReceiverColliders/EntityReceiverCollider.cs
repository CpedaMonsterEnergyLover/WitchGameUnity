using UnityEngine;

namespace Receivers
{
    // This collider must be on "default" layer and can be both trigger or not 
    [RequireComponent(typeof(Collider2D))]
    public class EntityRecieverCollider : ReceiverCollider<IEntityReceiver>
    {
        // Make sure that other collider is on "Entity" layer and
        // On the same GO with "Entity" script
        protected override void OnAnyCollisionEnter(GameObject otherGameObject)
        {
            if (otherGameObject.TryGetComponent(out Entity entity)) 
                receiver.OnReceiveEntity(entity);
        }

        protected override void OnAnyCollisionExit(GameObject otherGameObject)
        {
            if (otherGameObject.TryGetComponent(out Entity entity)) 
                receiver.OnEntityExitReceiver(entity);
        }
    }

}