﻿using System.Collections.Generic;
using System.Linq;
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
    
    public WorldData WorldData { get; protected set; }

    public List<Entity> Entities { get; private set; } = new();
    
    
    private void Awake()
    {
        playerTransform = FindObjectOfType<PlayerController>().transform;
        Application.targetFrameRate = playerSettings.targetFrameRate;
        Instance = this;
    }

 
    private void Start()
    {
        ClearAllTiles();
        ClearAllInteractable();
        LoadData();
    }

    private void LoadData()
    {
        WorldData loadedData = GameDataManager.LoadPersistentWorldData(worldScene);

        // Если файла мира еще нет, генерирует его
        if (loadedData is null)
        {
            Debug.Log($"Generating world {worldScene.sceneName}");
            GameDataManager.DeleteTemporaryData(worldScene);
            GenerateWorld();
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
                    loadedData.GetTile(tile.Position.x, tile.Position.y).MergeData(tile, true);
                Debug.Log($"{tempData.Count} merged;");
            }
            else
            {
                Debug.Log($"Temp file for {worldScene.sceneName} not found");

            }

            WorldData = loadedData;
        }
        
        SpawnPlayer();
    }
 

    protected virtual void SpawnPlayer()
    {
        int mapCenterX = WorldData.MapWidth / 2;
        int mapCenterY = WorldData.MapHeight / 2;
        playerTransform.position = new Vector3(mapCenterX, mapCenterY, 0f);
    }
    
    public virtual void GenerateWorld()
    {
        if (Application.isEditor)
        {
            gameCollectionManager.Init();
            Instance = this;
        }
        WorldData = generator.GenerateWorld(layers, worldScene);
        GameDataManager.SavePersistentWorldData(WorldData);
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
    
    public void AddEntity(WorldTile tile, EntitySaveData saveData)
    {
        if(saveData is null) return;
        
        if(tile.IsLoaded) tile.LoadEntities();

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
    }

    public void EraseTile(int x, int y)
    {
        Vector3Int position = new Vector3Int(x, y, 0);
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

    public bool TryGetTopLayer(int x, int y, out WorldLayer topLayer)
    {
        topLayer = null;
        var tileLayers = WorldData.GetTile(x, y).Layers;
        for (var i = 0; i < tileLayers.Length; i++)
            if (tileLayers[i])
                topLayer = layers[i];
        return topLayer is not null;
    }
    
    public WorldLayer GetTopLayer(int x, int y)
    {
        WorldLayer topLayer = null;
        var tileLayers = WorldData.GetTile(x, y).Layers;
        for (var i = 0; i < tileLayers.Length; i++)
            if (tileLayers[i])
                topLayer = layers[i];
        return topLayer;
    }

    #region Utils

    public bool CoordsBelongsToWorld(Vector2Int pos)
    {
        return WorldData.CoordsBelongsToWorld(pos.x, pos.y);
    }
    
    public bool CoordsBelongsToWorld(int x, int y)
    {
        return WorldData.CoordsBelongsToWorld(x, y);
    }

    #endregion
}
