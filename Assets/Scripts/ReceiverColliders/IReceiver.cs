public interface IReceiver
{
}

public interface IItemEntityReceiver : IReceiver
{
    void OnReceiveItemEntity(ItemEntity itemEntity);
    void OnItemEntityExitReceiver(ItemEntity itemEntity);
}

public interface IEntityReceiver : IReceiver
{
    void OnReceiveEntity(Entity entity);
    void OnEntityExitReceiver(Entity entity);
}

public interface IPlayerReceiver : IReceiver
{
    void OnReceivePlayer();
    void OnPlayerExitReceiver();
}

public interface IBulletReceiver : IReceiver
{
    void OnBulletReceive(Bullet bullet);
    void OnBulletExitReceiver(Bullet bullet);
}
