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
    private Generator generator;
    public bool generateOnStart = true;
    
    [Header("Слои грида")]
    public List<WorldLayer> layers;


    public TileLoader tileLoader;
    
    
    public WorldData WorldData { get; private set; }


    
    private void Awake()
    {
        Application.targetFrameRate = playerSettings.targetFrameRate;
        Instance = this;
        
        if (!generateOnStart) return;
        
        ClearAllTiles();
        GenerateWorld();
        
        int mapCenterX = generator.generatorSettings.width / 2;
        int mapCenterY = generator.generatorSettings.height / 2;
        playerTransform.position = new Vector3(mapCenterX, mapCenterY, 0f);
    }
    
    public void GenerateWorld()
    { 
        gameCollectionManager.Init();
        WorldData = generator.GenerateWorld(layers);
        Instance = this;
        tileLoader.Init(WorldData);
    }

    public void DrawAllTiles()
    {
        for (int x = 0; x < generator.generatorSettings.width; x++)
        {
            for (int y = 0; y < generator.generatorSettings.height; y++)
            {
                tileLoader.LoadTile(x, y);
            }
        }
    }

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

    public WorldLayerEditSettings GetTopLayerEditSettingsOrNull(int x, int y)
    {
        WorldLayerEditSettings layerEditSettings = null;
        var tileLayers = WorldData.GetTile(x, y).Layers;
        for (var i = 0; i < tileLayers.Length; i++)
        {
            if(tileLayers[i]) layerEditSettings = layers[i].layerEditSettings;
        }

        return layerEditSettings;
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
