using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;
    public new static Camera camera;
    private void Awake() => camera = mainCamera;

    public Transform playerTransform;
    public bool following;
    

    // Update is called once per frame
    private void Update()
    {
        if (following)
        {
            UpdatePosition();
        }
    }

    public void UpdatePosition()
    {
        var playerPos = playerTransform.position;
        var position = new Vector3(playerPos.x, playerPos.y + 0.5f, -10);
        transform.position = position;
    }
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