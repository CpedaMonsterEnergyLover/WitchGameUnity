using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Receivers
{
    public abstract class ReceiverCollider <T> : MonoBehaviour where T : IReceiver
    {
        [Header("Component-reciever (IReciever)")] 
        public Component receiverComponent;
        
        protected T receiver;

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