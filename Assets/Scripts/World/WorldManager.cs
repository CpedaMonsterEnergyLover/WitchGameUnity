using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using TileLoading;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;

    [SerializeField, Header("WorldScene")]
    public WorldScenes.BaseWorldScene worldScene;
    
    // [SerializeField, Header("Игрок")] 
    public Transform playerTransform;
    
    [Header("Игровые настройки")]
    public PlayerSettings playerSettings;

    public GameCollection.Manager gameCollectionManager;
    
    [Header("К чему крепить Interactable")]
    public Transform interactableTransform;
    public Transform entitiesTransform;
    
    [SerializeField, Header("Генератор")]
    protected Generator generator;
    
    [Header("Слои грида")]
    public List<WorldLayer> layers;
    public WorldBounds worldBounds;
    
    public WorldData WorldData { get; protected set; }

    private List<Entity> Entities { get; } = new();
    private Cache<Entity> EntityCache { get; set; }

    
    
    private void Awake()
    {
        playerTransform = FindObjectOfType<PlayerController>().transform;
        Application.targetFrameRate = playerSettings.targetFrameRate;
        EntityCache = new Cache<Entity>(playerSettings.entitiesCacheSize);
        Instance = this;
        
    }

    public void UnloadAllEntities()
    {
        foreach (Entity entity in Entities)
        {
            entity.SaveOnTile(out _);
            Destroy(entity.gameObject);
        }
        EntityCache.Clear();
    }

 
    private void Start()
    {
        ClearAllTiles();
        ClearAllInteractable();
        GameDataManager.DeleteTemporaryData(worldScene);
        LoadData();
        SpawnPlayer();
        ScreenFader.StopFade();
        worldBounds.Init(WorldData);
    }
    
    private void LoadData()
    {
        var loadedData = GameDataManager.LoadPersistentWorldData(worldScene);

        // Если файла мира еще нет, генерирует его
        if (loadedData is null)
        {
            Debug.Log($"Generating world {worldScene.sceneName}");
            GameDataManager.DeleteTemporaryData(worldScene);
            WorldData = TestGenerateWorld();
            GameDataManager.SavePersistentWorldData(WorldData);
        }
        
        // Если он уже есть
        else
        {
            var tempData = GameDataManager.LoadTemporaryWorldData(worldScene);
            // Если найден временный файл, мержит его
            if (tempData is not null)
            {
                Debug.Log($"Found temp file for {worldScene.sceneName}. Merging data...");
                foreach (WorldTile tile in tempData)
                    loadedData.GetTile(tile.Position.x, tile.Position.y)
                        .MergeData(tile, true);
                Debug.Log($"{tempData.Count} merged;");
            }
            else
            {
                Debug.Log($"Temp file for {worldScene.sceneName} not found, no merge needed");
            }

            WorldData = loadedData;
        }
        
    }
 


    
    
    protected virtual void SpawnPlayer()
    {
        var spawnPoint = WorldData.SpawnPoint;

        PlayerData playerData = PlayerManager.Instance.PlayerData;
        if (playerData is not null)
        {
            spawnPoint = playerData.Position;
        }
        playerTransform.position = new Vector3(spawnPoint.x, spawnPoint.y, 0f);
    }

    private WorldData TestGenerateWorld()
    {
        if (Application.isEditor)
        {
            gameCollectionManager.Init();
            Instance = this;
        }
        return generator.GenerateWorldData(layers, worldScene).GetAwaiter().GetResult();
    }
    
    public virtual async Task GenerateWorld()
    {
        if (Application.isEditor)
        {
            gameCollectionManager.Init();
            Instance = this;
        }
        WorldData = await generator.GenerateWorldData(layers, worldScene);
    }

    public void DrawAllTiles()
    {
        for (int x = 0; x < WorldData.MapWidth; x++)
        for (int y = 0; y < WorldData.MapHeight; y++)
            DrawTile(x, y);
    }

    public void DrawAllInteractable()
    {
        for (int x = 0; x < WorldData.MapWidth; x++)
        for (int y = 0; y < WorldData.MapHeight; y++)
            WorldData.GetTile(x, y).LoadInteractable();
    }

    public void ClearAllTiles()
    {
        layers.ForEach(layer => layer.tilemap.ClearAllTiles());
        ClearAllInteractable();
    }
    
    public void DrawTile(int x, int y)
    {
        Vector3Int position = new Vector3Int(x, y, 0);

        WorldTile tile = WorldData.GetTile(x, y);
        
        for (var i = 0; i < layers.Count; i++)
        {
            WorldLayer worldLayer = layers[i];
            worldLayer.tilemap.SetTile(position, 
                tile.Layers[i] ? worldLayer.tileBase : null);
        }

        if (WorldData.ColorLayerIndex != -1)
        {
            layers[WorldData.ColorLayerIndex].tilemap
                .SetColor(position, tile.Color);
        }
    }

    public void EraseTile(Vector3Int position)
    {
        layers.ForEach(layer =>
        {
            layer.tilemap.SetTile(position, null); 
        });
    }

    public void ClearAllInteractable()
    {
        while (interactableTransform.childCount > 0)
            DestroyImmediate(interactableTransform.GetChild(0).gameObject);
    }

    public bool TryGetTopLayer(WorldTile tile, out WorldLayer topLayer)
    {
        topLayer = null;
        var tileLayers = tile.Layers;
        for (var i = 0; i < tileLayers.Length; i++)
            if (tileLayers[i])
                topLayer = layers[i];
        return topLayer is not null;
    }
    
    public WorldLayer GetTopLayer(WorldTile tile)
    {
        WorldLayer topLayer = null;
        var tileLayers = tile.Layers;
        for (var i = 0; i < tileLayers.Length; i++)
            if (tileLayers[i])
                topLayer = layers[i];
        return topLayer;
    }

    public void CacheEntity([CanBeNull] Entity entity)
    {
        RemoveEntity(entity);
        EntityCache.Add(entity);
    }

    public void RemoveEntityFromCache([CanBeNull] Entity entity)
    {
        EntityCache.Remove(entity);
    }
    
    public void AddEntity(Entity entity)
    {
        if(!Entities.Contains(entity))
            Entities.Add(entity);
    }

    public void RemoveEntity(Entity entity)
    {
        if(Entities.Contains(entity))
            Entities.Remove(entity);
    }
    
}
