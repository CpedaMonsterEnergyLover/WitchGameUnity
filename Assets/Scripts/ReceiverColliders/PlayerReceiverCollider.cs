using UnityEngine;

namespace Receivers
{
    // This collider must be on "default" layer and can be both trigger or not 
    [RequireComponent(typeof(Collider2D))]
    public class PlayerReceiverCollider : ReceiverCollider<IPlayerReceiver>
    {
        
        // Make sure that other collider is on "Player" layer and
        // On the same GO with "PlayerController" script
        protected override void OnStayOrEnter(GameObject otherGameObject)
        {
            if (otherGameObject.TryGetComponent(out PlayerController _)) 
                receiver.OnReceivePlayer();
        }

        protected override void OnExit(GameObject otherGameObject)
        {
            if (otherGameObject.TryGetComponent(out PlayerController _)) 
                receiver.OnPlayerExitReceiver();
        }
    }

}