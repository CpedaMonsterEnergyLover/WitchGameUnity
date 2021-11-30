using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject bulletPrefab;

    public Transform playerTransform;
    public bool following;

    // Update is called once per frame
    void Update()
    {
        if (following)
        {
            Vector3 position = transform.position;
            position = new Vector3(playerTransform.position.x, playerTransform.position.y + 0.5f, position.z);
            transform.position = position;
        }
        
        if (Input.GetMouseButton(0))
        {
            // GameObject bullet = Instantiate(bulletPrefab);
            Vector3 targetPosition = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = 0;
            // bullet.transform.position = targetPosition;
            
            BulletPatternSpawner.Bomb(bulletPrefab, targetPosition, 10);
            
        }

    }
}
