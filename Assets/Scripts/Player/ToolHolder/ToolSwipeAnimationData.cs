using UnityEngine;

public readonly struct ToolSwipeAnimationData
{
    public readonly float speed;
    public readonly float cooldown;
    public readonly bool repeat;
    public readonly ToolSwipeAnimationType type;
    public readonly bool interruptable;

    public ToolSwipeAnimationData( ToolSwipeAnimationType type, float speed, float cooldown, bool repeat, bool interruptable)
    {
        this.speed = speed;
        this.cooldown = cooldown;
        this.repeat = repeat;
        this.interruptable = interruptable;
        this.type = type;
    }
}
