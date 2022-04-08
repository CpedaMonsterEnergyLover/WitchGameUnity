﻿using UnityEngine;

namespace Receivers
{
    // This collider must be on "default" layer and can be both trigger or not 
    [RequireComponent(typeof(Collider2D))]
    public class BulletReceiverCollider : ReceiverCollider<IBulletReceiver>
    {
        
        // Make sure that other collider is on "Bullet" layer and
        // On the same GO with "Bullet" script
        protected override void OnStayOrEnter(GameObject otherGameObject)
        {
            if (otherGameObject.TryGetComponent(out Bullet bullet)) 
                receiver.OnBulletReceive(bullet);
        }

        protected override void OnExit(GameObject otherGameObject)
        {
            if (otherGameObject.TryGetComponent(out Bullet bullet)) 
                receiver.OnBulletExitReceiver(bullet);
        }
    }

}