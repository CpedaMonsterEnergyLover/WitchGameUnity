using System.Collections;
using UnityEngine;

namespace Receivers
{
    // This collider must be on "default" layer and can be both trigger or not 
    [RequireComponent(typeof(Collider2D))]
    public class BulletReceiverCollider : ReceiverCollider<IBulletReceiver>
    {
        public bool receiveHostile;
        [SerializeField] private float receiveDelay = 0.5f;

        private bool delayed;

        public float Delay => receiveDelay;
        public void SetEnabled(bool isEnabled) => collider.enabled = isEnabled;
        
        protected override void OnAnyCollisionEnter(GameObject otherGameObject)
        {
            if (otherGameObject.TryGetComponent(out Bullet bullet))
            {
                switch (receiveHostile)
                {
                    case true when bullet.CompareTag("HostileBullet"):
                    {
                        if(!delayed) receiver.OnBulletReceive(bullet);
                        bullet.Kill();
                        break;
                    }
                    case false when bullet.CompareTag("Bullet"):
                    {
                        if(!delayed) receiver.OnBulletReceive(bullet);
                        bullet.Kill();
                        break;
                    }
                }

                if(gameObject.activeSelf) StartCoroutine(DelayRoutine());
            }
                
        }

        protected override void OnAnyCollisionExit(GameObject otherGameObject)
        {
            if (otherGameObject.TryGetComponent(out Bullet bullet)) 
                receiver.OnBulletExitReceiver(bullet);
        }

        private IEnumerator DelayRoutine()
        {
            delayed = true;
            yield return new WaitForSeconds(receiveDelay);
            delayed = false;
        }
    }

}