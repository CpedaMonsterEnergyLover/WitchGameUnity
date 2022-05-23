using System;
using UnityEngine;

public interface IWorldTransitionInitiator
{
    public object[] TransitionData { get; }
    public Vector2 TransitionPosition { get; }
    public Action TransitionCallback { get; }
}
