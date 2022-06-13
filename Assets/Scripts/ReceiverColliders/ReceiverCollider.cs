using UnityEngine;

namespace Receivers
{
    public abstract class ReceiverCollider <T> : MonoBehaviour where T : IReceiver
    {
        [Header("Component-reciever (IReciever)")] 
        [SerializeField] private Component receiverComponent;
        
        protected T receiver;
        protected new Collider2D collider;
        
        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void Start()
        {
            if (receiverComponent is not T component)
            {
                Debug.LogAssertion(
                    $"Component {receiverComponent.GetType()} is not implementing {typeof(T)}", this);
                gameObject.SetActive(false);
            }
            else
            {
                collider = GetComponent<Collider2D>();
                receiver = component;
                gameObject.layer = LayerMask.NameToLayer("Receiver");
            }
        }

        private void OnTriggerEnter2D(Collider2D other) => OnAnyCollisionEnter(other.gameObject);
        private void OnCollisionEnter2D(Collision2D other) => OnAnyCollisionEnter(other.gameObject);

        private void OnTriggerExit2D(Collider2D other) => OnAnyCollisionExit(other.gameObject);
        private void OnCollisionExit2D(Collision2D other) => OnAnyCollisionExit(other.gameObject);

        protected abstract void OnAnyCollisionEnter(GameObject otherGameObject);
        protected abstract void OnAnyCollisionExit(GameObject otherGameObject);
    }
}