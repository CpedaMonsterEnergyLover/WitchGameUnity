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
    public float minSpeed = 2.0f;
    public float maxSpeed = 2.0f;
    
}

[System.Serializable]
public class HomingProperties
{
    public bool isHoming = false;
    public float homingSpeed = 0.5f;
}

public enum Direction
{
    Custom,
    ToPlayer,
    FromPlayer,
}