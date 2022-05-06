using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public int maxHearts;
    public InventoryWindow inventoryWindow;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private SpriteRenderer spriteRenderer;

    
    public PlayerData PlayerData { get; private set; }
    public int Health { get; private set; }

    public Vector2 Position
    {
        get => playerTransform.position;
        set => playerTransform.position = value;
    }

    public Transform Transform => playerTransform;
    public Vector3 Position3 => playerTransform.position;
    public Vector2Int TilePosition 
        => new(Mathf.FloorToInt(playerTransform.position.x),
                Mathf.FloorToInt(playerTransform.position.y));
    public SpriteRenderer PlayerSpriteRenderer => spriteRenderer;


    public delegate void HeartAddEvent(Heart heart, int index);
    public delegate void HeartRemoveEvent(int index);

    public void SetPosition(Vector2 position) => playerTransform.position = position;
    
    private void Awake()
    {
        Instance = this;
        PlayerData = GameDataManager.LoadPlayerData();
        if (PlayerData is not null)
        {
            Health = PlayerData.Health;
            playerTransform.position = PlayerData.Position;
            playerData = PlayerData;
        }
    }
    
}
