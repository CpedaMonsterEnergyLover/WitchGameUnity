using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class BulletSpawner : MonoBehaviour
{
    public void Bomb(GameObject prefab, Vector3 position, int count, float radius, float duration, bool moveSimultaniously)
    {
        StartCoroutine(BulletSpawnerRoutine(prefab, position, count, radius, duration, moveSimultaniously));
    }

    /*private static Bullet BombPattern()
    {
        Vector3 randomPosition = position + (Vector3) Random.insideUnitCircle * radius;
        return Instantiate(prefab, randomPosition, Quaternion.identity)
            .GetComponent<Bullet>();
    }*/
    
    private static IEnumerator BulletSpawnerRoutine(
        GameObject prefab, Vector3 position, 
        int count, float radius, float duration, bool moveSimultaniously)
    {
        BulletPool bulletPool = new BulletPool();
        
        float delay = duration / count;

        for (int i = 0; i < count; i++)
        {
            
            Vector3 randomPosition = position + (Vector3) Random.insideUnitCircle * radius;
            Bullet bullet = Instantiate(prefab, randomPosition, Quaternion.identity)
                .GetComponent<Bullet>();

            if(moveSimultaniously) bulletPool.Add(bullet);

            yield return new WaitForSeconds(delay);
        }
        
        if (moveSimultaniously) bulletPool.ShootAll();
    }

}

public enum BulletPattern
{
    Star,
    Bomb,
    Circle,
    Square
}
