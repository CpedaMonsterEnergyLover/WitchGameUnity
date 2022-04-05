using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;
    
    [SerializeField, Header("Игрок")] public Transform playerTransform;
    
    [Header("Игровые настройки")]
    public PlayerSettings playerSettings;

    public GameCollection.Manager gameCollectionManager;
    
    [Header("К чему крепить Interactable")]
    public Transform interactableTransform;
    public Transform entitiesTransform;
    
    [SerializeField, Header("Генератор")]
    protected Generator generator;
    public bool generateOnStart = true;
    
    [Header("Слои грида")]
    public List<WorldLayer> layers;


    
    
    public WorldData WorldData { get; protected set; }


    
    private void Awake()
    {
        Application.targetFrameRate = playerSettings.targetFrameRate;
        Instance = this;
        
        if (!generateOnStart) return;
        
        ClearAllTiles();
        GenerateWorld();
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
        gameCollectionManager.Init();
        WorldData = generator.GenerateWorld(layers);
        Instance = this;
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

    // Runtime only
    public void AddInteractable(WorldTile tile, InteractableSaveData saveData)
    {
        if(saveData is null) return;
        WorldData.AddInteractableObject(tile.Position, saveData);
        tile.LoadInteractable();
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
