using InteractableInterfaces;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ItemEntityReceiverCollider : MonoBehaviour
{
    [Header("IItemEntityReceiver")]
    public Component receiverComponent;
    private IItemEntityReceiver _receiver;
    
    private void Start()
    {
        if (receiverComponent is not IItemEntityReceiver receiver)
        {
            Debug.LogAssertion($"Объект {name} содержит скрипт ItemEntityReceiverCollider без IItemEntityReceiver'a");
            Destroy(gameObject);
        }
        else
        {
            _receiver = receiver;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out ItemEntity itemEntity))
            _receiver.OnReceiveItemEntity(itemEntity);
    }
}
