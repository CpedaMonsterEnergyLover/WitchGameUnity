using UnityEngine;

namespace InteractableInterfaces
{
    public interface IItemEntityReceiver
    {
        void OnReceiveItemEntity(ItemEntity itemEntity);
    }
}