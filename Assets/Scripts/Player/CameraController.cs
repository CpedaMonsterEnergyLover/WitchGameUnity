using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject bulletPrefab;

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
        
        /*Vector3 targetPosition = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = 0;


        if (Input.GetMouseButtonDown(1))
        {
            Vector3Int targetPositionInt = Vector3Int.FloorToInt(targetPosition);
            if (WorldManager.CoordsBelongsToWorld(targetPositionInt.x, targetPositionInt.y))
            {
                WorldTile tile = WorldManager.WorldData.GetTile(targetPositionInt.x, targetPositionInt.y);
                if (tile.HasInteractable) return;
                WorldManager.AddInteractable(targetPositionInt, new InteractableIdentifier(InteractableType.Herb, "calendula"));
            }
        }*/

        /*if (Input.GetMouseButton(1))
        {
            BulletSpawner.Instance.Bomb(bulletPrefab, targetPosition, count, radius, duration, moveSimultaniously);
        } 
        
        if (Input.GetMouseButtonDown(0))
        {
            // BulletSpawner.Instance.Circle(bulletPrefab, targetPosition, count, radius, angleStart, angleEnd,duration, moveSimultaniously);
            // BulletSpawner.Instance.Spiral(bulletPrefab, targetPosition, count, countOfTurns, duration, angleStart);
        }*/

    }
}
