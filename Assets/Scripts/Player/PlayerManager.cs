using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public InventoryWindow inventoryWindow;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public PlayerData PlayerData { get; private set; }
    public int Health { get; private set; }
    public Vector3 Position => playerTransform.position;

    public Vector2Int TilePosition => new Vector2Int(
        Mathf.FloorToInt(playerTransform.position.x),
        Mathf.FloorToInt(playerTransform.position.y));

    public SpriteRenderer PlayerSpriteRenderer => spriteRenderer;
    
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
