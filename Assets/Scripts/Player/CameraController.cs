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
        
        if (Input.GetMouseButton(1))
        {
            // GameObject bullet = Instantiate(bulletPrefab);
            Vector3 targetPosition = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = 0;
            // bullet.transform.position = targetPosition;

            bulletSpawner.Bomb(bulletPrefab, targetPosition, count, radius, duration, moveSimultaniously);

        }
        
        if (Input.GetMouseButtonDown(0))
        {
            // GameObject bullet = Instantiate(bulletPrefab);
            Vector3 targetPosition = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = 0;
            // bullet.transform.position = targetPosition;

            // bulletSpawner.Bomb(bulletPrefab, targetPosition, count, radius, duration, false);
            bulletSpawner.Circle(bulletPrefab, targetPosition, count, radius, angleStart, angleEnd,duration, moveSimultaniously);

        }

    }
}
