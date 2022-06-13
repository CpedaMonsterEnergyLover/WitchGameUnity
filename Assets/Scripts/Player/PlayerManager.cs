using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public InventoryWindow inventoryWindow;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private HungerController hungerController;
    [SerializeField] private HeartContainer heartContainer;
    [SerializeField] private DeathManager deathManager;

    public void ApplyDamage()
    {
        heartContainer.ApplyDamage(DamageType.Physical);
    }

    public void ApplyEffect(HeartEffectData data) => heartContainer.ApplyEffect(0, data);
    public int Health => heartContainer.GetCount;
    public float CurrentHunger => hungerController.Hunger;
    public bool CanEat => hungerController.Hunger < hungerController.MaxHunger;
    public Vector2 Pos2D => playerTransform.position;
    public Vector3 Pos3D => playerTransform.position;
    public Transform Transform => playerTransform;

    public Vector2Int TilePosition 
        => new(Mathf.FloorToInt(playerTransform.position.x),
            Mathf.FloorToInt(playerTransform.position.y));
    public SpriteRenderer SpriteRenderer => spriteRenderer;
    
    public void SetPosition(Vector2 position)
    {
        TileLoader.Instance.ResetPreviousPosition();
        playerTransform.position = position;
    }

    public void AddItem(ItemStack itemStack) => inventoryWindow.AddItem(itemStack.item.identifier, itemStack.amount);

    public void AddHunger(float amount) => hungerController.AddHunger(amount);
    public void SetHunger(float amount) => hungerController.SetHunger(amount);

    public void AddHeart() =>
        heartContainer.Add(Heart.Create(HeartOrigin.Human, HeartType.Solid));

    
    private void Awake()
    {
        Instance = this;
        PlayerData playerData = GameDataManager.PlayerData;
        if (playerData is not null)
        {
            hungerController.Init(playerData.Hunger);
            playerTransform.position = playerData.Position;
            WorldPositionProvider.WorldIndex = playerData.CurrentSubWorldIndex;
            for (int i = 0; i < playerData.Health; i++) heartContainer.Add(Heart.Create(HeartOrigin.Human, HeartType.Solid));
        }
        else
        {
            for (int i = 0; i < 3; i++) heartContainer.Add(Heart.Create(HeartOrigin.Human, HeartType.Solid));
            hungerController.Init(hungerController.MaxHunger);
        }
    }

    private void Die() => deathManager.Die();
    
    private void Start()
    {
        heartContainer.ONDeath += Die;
        hungerController.ONHungerLow += Starve;
        hungerController.ONHungerArise += StopStarving;
        DeathManager.ONPlayerDeath += StopStarving;
    }

    private void OnDestroy()
    {
        heartContainer.ONDeath -= Die;
        hungerController.ONHungerLow -= Starve;
        hungerController.ONHungerArise -= StopStarving;
        DeathManager.ONPlayerDeath -= StopStarving;
    }

    private int _starveMinutes = 0;
    
    private void Starve()
    {
        _starveMinutes++;
        if(_starveMinutes % 30 == 0) ApplyDamage();
    }

    private void StopStarving()
    {
        _starveMinutes = 0;
    }
}