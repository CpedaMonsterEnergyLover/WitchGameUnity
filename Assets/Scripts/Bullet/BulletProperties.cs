using UnityEngine;

[System.Serializable]
public class BulletHitProperties
{
    public bool ignoreObstacles;
}

[System.Serializable]
public class BulletMovementProperties
{
    public Direction direction = Direction.ToPlayer;
    public Vector2 forceDirectionVector = Vector2.zero;
    public float speed = 2.0f;
}

public enum Direction
{
    Custom,
    ToPlayer,
    FromPlayer,
}