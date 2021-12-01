
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BulletSpawner : MonoBehaviour
{
    public void Bomb(GameObject prefab, Vector3 position, int count, float radius, float duration, bool moveSimultaniously)
    {
        StartCoroutine(BombPattern(prefab, position, count, radius, duration, moveSimultaniously));
    }

    public void Circle(GameObject prefab, Vector3 position,
        int count, float radius, float startAngle, float endAngle, float duration, bool moveSimultaniously)
    {
        StartCoroutine(CirclePattern(prefab, position, count, radius,  startAngle, endAngle, duration, moveSimultaniously));
    }

    public void Spiral(GameObject prefab, Vector3 position, int bulletsPerTurn, int countOfTurns, float duration,
        float startAngle)
    {
        int count = bulletsPerTurn * countOfTurns;
        float radius = 0.25f;
        float endAngle = startAngle + 360 * countOfTurns;
        bool moveSimultaniously = false;
        StartCoroutine(CirclePattern(prefab, position, count, radius,  startAngle, endAngle, duration, moveSimultaniously));
    }

    private static IEnumerator BombPattern(
        GameObject prefab, Vector3 position, 
        int count, float radius, float duration, bool moveSimultaniously)
    {
        BulletGroup bulletGroup = new BulletGroup();
        
        float delay = duration / count;

        for (int i = 0; i < count; i++)
        {
            
            Vector3 randomPosition = position + (Vector3) Random.insideUnitCircle * radius;
            Bullet bullet = Instantiate(prefab, randomPosition, Quaternion.identity)
                .GetComponent<Bullet>();

            if(moveSimultaniously) bulletGroup.Add(bullet);
            
            if (duration != 0.0f) yield return new WaitForSeconds(delay);

        }
        
        if (moveSimultaniously) bulletGroup.ShootAll();
    }


    private static IEnumerator CirclePattern(
        GameObject prefab, Vector3 position,
        int count, float radius, float startAngle, float endAngle, float duration, bool moveSimultaniously)
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

            Bullet bullet = Instantiate(prefab, newPosition, Quaternion.identity)
                .GetComponent<Bullet>();
            bullet.movementProperties.direction = Direction.Custom;
            bullet.movementProperties.forceDirectionVector = direction;

            currentAngle += angleStep;

            if (moveSimultaniously) bulletGroup.Add(bullet);    

            if (duration != 0.0f) yield return new WaitForSeconds(delay);
        }
        
        if (moveSimultaniously) bulletGroup.ShootAll();
    }
}