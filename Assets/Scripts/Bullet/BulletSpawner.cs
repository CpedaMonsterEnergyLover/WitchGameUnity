
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BulletSpawner : MonoBehaviour
{
    #region Singleton

    public static BulletSpawner Instance;

    private void Awake() => Instance = this;

    #endregion
    
    
    public void Bomb(
        GameObject prefab, 
        Vector3 position, 
        int count, 
        float radius, 
        float duration, 
        bool moveSimultaniously,
        Action onEnd)
    {
        StartCoroutine(BombPattern(prefab, position, count, radius, duration, moveSimultaniously, onEnd));
    }

    public void Circle(
        GameObject prefab, 
        Vector3 position,
        int count, 
        float radius, 
        float startAngle,
        float duration, 
        bool moveSimultaniously, 
        Action onEnd)
    {
        StartCoroutine(CirclePattern(prefab, position, count, radius,  startAngle, 360 + startAngle, duration, moveSimultaniously, onEnd));
    }

    public void Spiral(
        GameObject prefab, 
        Vector3 position, 
        int bulletsPerTurn, 
        int countOfTurns, 
        float duration,
        float startAngle, 
        Action onEnd)
    {
        int count = bulletsPerTurn * countOfTurns;
        float radius = 0.25f;
        float endAngle = startAngle + 360 * countOfTurns;
        StartCoroutine(CirclePattern(prefab, position, count, radius,  startAngle, endAngle, duration, false, onEnd));
    }

    private static IEnumerator BombPattern(
        GameObject prefab, Vector3 position, 
        int count, float radius, float duration, bool moveSimultaniously, Action onEnd = null)
    {
        BulletGroup bulletGroup = new BulletGroup();
        
        float delay = duration / count;

        for (int i = 0; i < count; i++)
        {
            
            Vector3 randomPosition = position + (Vector3) Random.insideUnitCircle * radius;
            Bullet bullet = SingleBullet(prefab, randomPosition);

            if(moveSimultaniously) bulletGroup.Add(bullet);
            
            if (duration != 0.0f) yield return new WaitForSeconds(delay);

        }
        
        if (moveSimultaniously) bulletGroup.ShootAll();
        onEnd?.Invoke();
    }


    private static IEnumerator CirclePattern(
        GameObject prefab, Vector3 position,
        int count, float radius, float startAngle, float endAngle, float duration, bool moveSimultaniously, Action onEnd = null)
    {
        BulletGroup bulletGroup = new BulletGroup();

        float delay = duration / count;
        float angleStep = (endAngle - startAngle) / count;
        float currentAngle = startAngle;

        for (int i = 0; i < count; i++)
        {

            float x = position.x + Mathf.Sin(currentAngle * Mathf.PI / 180f) * radius;
            float y = position.y + Mathf.Cos(currentAngle * Mathf.PI / 180f) * radius;

            Vector3 newPosition = new Vector3(x, y, 0f);
            Vector2 direction = (newPosition - position).normalized;

            Bullet bullet = SingleBullet(prefab, newPosition);
            bullet.movementProperties.direction = Direction.Custom;
            bullet.movementProperties.forceDirectionVector = direction;

            currentAngle += angleStep;

            if (moveSimultaniously) bulletGroup.Add(bullet);    

            if (duration != 0.0f) yield return new WaitForSeconds(delay);
        }
        
        if (moveSimultaniously) bulletGroup.ShootAll();
        onEnd?.Invoke();
    }

    public static Bullet SingleBullet(GameObject prefab, Vector3 position)
    {
        return Instantiate(prefab, position, Quaternion.identity)
            .GetComponent<Bullet>();
    }
}