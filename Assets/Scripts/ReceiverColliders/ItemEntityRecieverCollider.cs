using UnityEngine;

namespace Receivers
{
    // This collider must be on "default" layer and can be both trigger or not 
    [RequireComponent(typeof(Collider2D))]
    public class ItemEntityRecieverCollider : ReceiverCollider<IItemEntityReceiver>
    {
        
        // Make sure that other collider is on "ItemTrigger" layer and
        // On the same GO with "ItemEntity" script
        protected override void OnStayOrEnter(GameObject otherGameObject)
        {
            if (otherGameObject.TryGetComponent(out ItemEntity itemEntity)) 
                receiver.OnReceiveItemEntity(itemEntity);
        }

        protected override void OnExit(GameObject otherGameObject)
        {
            if (otherGameObject.TryGetComponent(out ItemEntity itemEntity)) 
                receiver.OnItemEntityExitReceiver(itemEntity);
        }
    }

}