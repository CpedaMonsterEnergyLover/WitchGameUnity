using UnityEngine;
using Random = UnityEngine.Random;

public static class BulletPatternSpawner
{

    public static void Bomb(GameObject prefab, Vector3 position, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Bullet bullet = Object.Instantiate(prefab, position, Quaternion.identity)
                .GetComponent<Bullet>();
            bullet.movementProperties.direction = Direction.Custom;
            bullet.movementProperties.forceDirectionVector = 
                (Vector2) /*bullet.gameObject.transform.position +*/ Random.insideUnitCircle.normalized * 1;
        }
    }
    
}

public enum BulletPattern
{
    Star,
    Bomb,
    Circle,
    Square
}
