using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public InventoryWindow inventoryWindow;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    public int Health { get; private set; }
    
    public Vector2 Pos2D => playerTransform.position;
    public Vector3 Pos3D => playerTransform.position;
    public Transform Transform => playerTransform;

    public Vector2Int TilePosition 
        => new(Mathf.FloorToInt(playerTransform.position.x),
                Mathf.FloorToInt(playerTransform.position.y));
    public SpriteRenderer SpriteRenderer => spriteRenderer;
    
    public void SetPosition(Vector2 position) => playerTransform.position = position;
    
    private void Awake()
    {
        Instance = this;
        PlayerData playerData = GameDataManager.PlayerData;
        if (playerData is not null)
        {
            Health = playerData.Health;
            playerTransform.position = playerData.Position;
            WorldPositionProvider.WorldIndex = playerData.CurrentSubWorldIndex;
        }
    }
    
}
