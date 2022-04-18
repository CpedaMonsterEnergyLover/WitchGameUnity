using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Transform playerTransform;
    public InventoryWindow inventoryWindow;
    [SerializeField] private PlayerData playerData;
    public static PlayerManager Instance;
    
    public PlayerData PlayerData { get; private set; }
    public int Health { get; private set; }
    public Vector3 Position => playerTransform.position;
    
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
