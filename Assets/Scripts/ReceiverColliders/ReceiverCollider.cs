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
        public bool onStay;
        public float onStayTickDelay;
        
        protected T receiver;
        private Coroutine _routine;

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
        
        private void OnAnyCollisionEnter(GameObject otherGameObject)
        {
            if (!onStay) OnStayOrEnter(otherGameObject);
            else _routine = StartCoroutine(OnStayRoutine(otherGameObject));
        }
        
        private void OnAnyCollisionExit(GameObject otherGameObject)
        {
            if(_routine is not null) StopCoroutine(_routine);
            OnExit(otherGameObject);
        }

        private IEnumerator OnStayRoutine(GameObject otherGameObject)
        {
            while (true)
            {
                if (otherGameObject is null) yield break;
                OnStayOrEnter(otherGameObject);
                yield return new WaitForSecondsRealtime(onStayTickDelay);
            }
            // ReSharper disable once IteratorNeverReturns
        }

        protected abstract void OnStayOrEnter(GameObject otherGameObject);
        protected abstract void OnExit(GameObject otherGameObject);
    }
}