using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public BulletSpawner bulletSpawner;

    public Transform playerTransform;
    public bool following;
    
    [Header("Bullet settings")]
    public float radius;
    public float duration;
    public int count;
    public int countOfTurns;
    public int angleStart;
    public int angleEnd;
    public bool moveSimultaniously;

    // Update is called once per frame
    void Update()
    {
        if (following)
        {
            Vector3 position = transform.position;
            position = new Vector3(playerTransform.position.x, playerTransform.position.y + 0.5f, position.z);
            transform.position = position;
        }
        
        Vector3 targetPosition = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = 0;
        
        /*if (Input.GetMouseButton(1))
        {
            bulletSpawner.Bomb(bulletPrefab, targetPosition, count, radius, duration, moveSimultaniously);
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            // bulletSpawner.Circle(bulletPrefab, targetPosition, count, radius, angleStart, angleEnd,duration, moveSimultaniously);
            bulletSpawner.Spiral(bulletPrefab, targetPosition, count, countOfTurns, duration, angleStart);
        }*/

    }
}
