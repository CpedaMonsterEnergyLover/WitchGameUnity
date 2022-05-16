﻿using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using TileLoading;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance { get; protected set; }

    [Header("WorldScene")]
    public BaseWorldScene worldScene;
    
    [Header("Игровые настройки")]
    public PlayerSettings playerSettings;

    public GameCollection.Manager gameCollectionManager;
    
    [Header("К чему крепить Interactable")]
    public Transform interactableTransform;
    public Transform entitiesTransform;
    
    [SerializeField, Header("Генератор")]
    protected AbstractGenerator generator;
    
    [Header("Слои грида")]
    public List<WorldLayer> layers;
    public bool hasBounds;
    public WorldBounds worldBounds;
    
    public WorldData WorldData { get; protected set; }

    private List<Entity> Entities { get; } = new();
    private Cache<Entity> EntityCache { get; set; }


    public delegate void WorldLoadingEvent();
    public static event WorldLoadingEvent ONWorldLoaded;
    
    
    private void Awake()
    {
        Application.targetFrameRate = playerSettings.targetFrameRate;
        EntityCache = new Cache<Entity>(playerSettings.entitiesCacheSize);
        Instance = this;
    }
    
    private void Start()
    {
        ScreenFader.Instance.PlayBlack();
        ClearAllTiles();
        ClearAllInteractable();
        GameDataManager.DeleteTemporaryData(worldScene);
        PostStart().Forget();
    }

    private async UniTask PostStart()
    {
        await UniTask.NextFrame();
        await LoadData();
        ONWorldLoaded?.Invoke();
        if(hasBounds) worldBounds.Init(WorldData);
        PlayerManager.Instance.Position = GetPlayerSpawn();
        await ScreenFader.Instance.StopFade(0.5f);
    }
    
    private async UniTask LoadData()
    {
        var loadedData = GameDataManager.LoadPersistentWorldData(worldScene);
        
        // Если файла мира еще нет, генерирует его
        if (loadedData is null)
        {
            GameDataManager.DeleteTemporaryData(worldScene);
            if (Application.isEditor) Instance = this;
            WorldData = await generator.GenerateWorldData(layers, worldScene);
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
            WorldSettingsProvider.SetSettings(WorldData.WorldSettings);
        }
        
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

    public async UniTask GenerateFromEditor()
    {
        Instance = this;
        WorldData = await generator.GenerateWorldData(layers, worldScene, true);
    }

    
    protected virtual Vector2 GetPlayerSpawn()
    {
        Vector2 spawnPoint = WorldData.SpawnPoint;

        PlayerData playerData = PlayerManager.Instance.PlayerData;
        if (playerData is not null)
        {
            spawnPoint = playerData.Position;
        }

        return spawnPoint;
    }
    
    public void DrawAllTiles()
    {
        /*IEnumerator Routine()
        {
            for (int x = 0; x < WorldData.MapWidth; x++)
            for (int y = 0; y < WorldData.MapHeight; y++)
            {
                DrawTile(x, y);
                yield return new WaitForSecondsRealtime(0.1f);
            } 
        }

        StartCoroutine(Routine());*/
        
        for (int x = 0; x < WorldData.MapWidth; x++)
        for (int y = 0; y < WorldData.MapHeight; y++)
        {
            DrawTile(x, y);
        } 
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
        WorldData = null;
    }
    
    public void DrawTile(int x, int y)
    {
        Vector3Int position = new Vector3Int(x, y, 0);

        WorldTile tile = WorldData.GetTile(x, y);
        
        for (var i = 0; i < layers.Count; i++)
        {
            WorldLayer layer = layers[i];
            layer.tilemap.SetTile(position, 
                tile.Layers[i] ? layer.tileBase : null);
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
    
    public bool TryGetEditableTopLayer(WorldTile tile, out EditableWorldLayer topLayer)
    {
        topLayer = null;
        var tileLayers = tile.Layers;
        for (var i = 0; i < tileLayers.Length; i++)
            if (tileLayers[i])
                topLayer = layers[i] is EditableWorldLayer editableWorldLayer? editableWorldLayer : null;
        return topLayer is not null;
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
