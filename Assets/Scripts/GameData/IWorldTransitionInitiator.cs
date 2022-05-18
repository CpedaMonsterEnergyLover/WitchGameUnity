using UnityEngine;

public interface IWorldTransitionInitiator
{
    public object[] WorldTransitionInitiatorData { get; }
    public Vector2 SpawnPosition { get; }
}
