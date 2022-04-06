using EntityInterfaces;

namespace InteractableInterfaces
{
    public interface IHasFireCollider
    {
        ItemEntityReceiverCollider ItemEntityReceiverCollider { get; }
        void OnFireCollision(IFireDamagable entity);
    }
}