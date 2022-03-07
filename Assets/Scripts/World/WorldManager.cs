using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class WorldManager : MonoBehaviour
{
    #region Vars

    public static WorldManager Instance;
    
    // Public fields
    public Generator generator;
    public Transform playerTransform;
    public GameCollection.Manager gameCollectionManager;
    public int targetFrameRate = 60;
    [Range(1,50)]
    public int viewRangeX;
    [Range(1,50)]
    public int viewRangeY;
    [Range(1, 1000)]
    public int tileCacheSize;
    [Header("Грид")]
    public Grid worldGrid;
    [Header("Слои грида")]
    public Tilemap GroundTilemap;
    public Tilemap SandTilemap;
    public Tilemap WaterTilemap;
    public Tilemap PlainsTilemap;
    public Tilemap TreeTilemap;
    [Header("Тайлы")]
    public TileBase fertileGrassTile;
    public TileBase forestGrassTile;
    public TileBase sandTile;
    public TileBase waterTile;
    public TileBase swampTile;
    public TileBase plainsGrassTile;

    public WorldData worldData;

    
    // Private fields
    [FormerlySerializedAs("_gameObjectsTransform")] [Header("К чему крепить все объекты"), SerializeField]
    public Transform gameObjectsTransform;
    private Tilemap[] _tilemapByEnumIndex;
    private TileBase[] _tilebaseByEnumIndex;

    // Tile cache
    public TileCache tileCache;
    private List<Vector3Int> _loadedTiles;
    
    // Properties
    public int CurrentCacheSize => tileCache?.Size ?? 0;
    public int CurrentLoadedTilesAmount => _loadedTiles?.Count ?? 0;
    
    #endregion


    
    #region UnityMethods

    private void Awake()
    {
        Application.targetFrameRate = targetFrameRate;

        if (Instance is null) Instance = this;
        else Debug.LogError("Found multiple instances of WorldManager", this);
        
        /*// Инициализация коллекции игровых объектов
        gameObjectsCollection.InitCollection();*/
    }

    private void Start()
    {
        if (generator.GenerateOnStart)
        {
            Generate();
            worldGrid.transform.position = new Vector3(0f, 0f, 0);
            int mapCenterX = generator.mapWidth / 2;
            int mapCenterY = generator.mapHeight / 2;
            playerTransform.position = new Vector3(mapCenterX, mapCenterY, 0f);
        }
    }

    private void Update()
    {
        if (!generator.GenerateOnStart) return;
        
        tileCache.SetMaxSize(tileCacheSize);
        
        // Прогрузка тайлов вокруг игрока
        Vector3Int playerPosition = Vector3Int.FloorToInt(playerTransform.position);
        for (int x = - viewRangeX; x <= viewRangeX; x++)
        {
            for (int y = - viewRangeY; y <= viewRangeY; y++)
            {
                int targetX = x + playerPosition.x;
                int targetY = y + playerPosition.y;
                // Проверка на выход за пределы карты
                if (!CoordsBelongsToWorld(targetX, targetY)) continue;
                
                // Если тайл еще не загружен, загружает его
                WorldTile tile = worldData.GetTile(targetX, targetY);
                if (tile.loaded) continue;
                LoadTile(targetX, targetY);
            }
        }

        // Составляет список координат тайлов, которы нужно убрать
        // Из поля зрения игрока
        List<Vector3Int> toRemove = new();
        _loadedTiles.ForEach(tile =>
        {
            Vector3Int target = playerPosition - tile;
            if (Math.Abs(target.x) > viewRangeX || Math.Abs(target.y) > viewRangeY)
                toRemove.Add(tile);
        });
        
        // Очищает все тайлы из составленного списка
        toRemove.ForEach(tilePosition =>
        {
            RemoveTile(tilePosition.x, tilePosition.y);
            _loadedTiles.Remove(tilePosition);
        });
        toRemove.Clear();
    }

    #endregion



    #region ClassMethods

    public void Generate()
    {
        Instance ??= this;
        
        // TODO: вынести инородные инициализации из этого метода в более подходящее место
        // Сейчас вся инициализация помещена в генерацию потому что требуется нажимать
        // Эту кнопку изнутри юнити
        
        // Инициализация Кеша
        tileCache = new TileCache(tileCacheSize);
            
        // Инициализация коллекции игровых объектов
        gameCollectionManager.Init();
        
        // Инициализация индексов слоев грида и тайлов
        InitTileIndexArrays();
        
        _loadedTiles = new List<Vector3Int>();
        
        ClearWorld();
        worldData = generator.GenerateWorld();
    }

    public void DrawAllTiles()
    {
        for (int x = 0; x < worldData.MapWidth; x++)
        {
            for (int y = 0; y < worldData.MapHeight; y++)
            {
                // loadedTiles.Add(new Vector3Int(x, y, 0));
                LoadTile(x, y);
            }
        }
    }

    private void LoadTile(int x, int y)
    {
        WorldTile tile = worldData.GetTile(x, y);
        tile.Load(_tilemapByEnumIndex, _tilebaseByEnumIndex);
        tileCache.Remove(tile);
        _loadedTiles.Add(new Vector3Int(x, y, 0));
    }

    private void RemoveTile(int x, int y)
    {
        WorldTile tile = worldData.GetTile(x, y);
        // Очищает грид
        tile.Erase(_tilemapByEnumIndex);
        if (tile.HasInteractable)
        {
            // Кеширует тайл
            tileCache.Add(tile);
        }

        tile.loaded = false;
        // Убирать из loadedTiles не надо, тк это происходит в цикле апдейта
    }

    public void ClearWorld()
    {
        ClearAllTiles();
        ClearAllInteractable();
    }

    private void ClearAllTiles()
    {
        GroundTilemap.ClearAllTiles();
        WaterTilemap.ClearAllTiles();
        SandTilemap.ClearAllTiles();
        PlainsTilemap.ClearAllTiles();
    }

    private void ClearAllInteractable()
    {
        while (gameObjectsTransform.childCount > 0)
            DestroyImmediate(gameObjectsTransform.GetChild(0).gameObject);
    }

    public void AddInteractable(WorldTile tile, InteractableSaveData saveData)
    {
        if(saveData is null) return;
        worldData.AddInteractableObject(tile.position, saveData);
        tile.LoadInteractable();
    }

    #endregion



    #region Utils

    private void InitTileIndexArrays()
    {
        _tilemapByEnumIndex = CreateTilemapByEnumIndexArray();
        _tilebaseByEnumIndex = CreateTilebaseByEnumIndexArray();
    }

    private Tilemap[] CreateTilemapByEnumIndexArray()
    {
        Tilemap[] tileMaps = new Tilemap[6];
        tileMaps[(int)GridLayer.Ground] = GroundTilemap;
        tileMaps[(int)GridLayer.Plains] = PlainsTilemap;
        tileMaps[(int)GridLayer.Sand] = SandTilemap;
        tileMaps[(int)GridLayer.Water] = WaterTilemap;
        return tileMaps;
    }

    private TileBase[] CreateTilebaseByEnumIndexArray()
    {
        TileBase[] tileBases = new TileBase[6];
        tileBases[(int)SoilType.Water] = waterTile;
        tileBases[(int)SoilType.Swamp] = swampTile;
        tileBases[(int)SoilType.Sand] = sandTile;
        tileBases[(int)SoilType.FertileGrass] = fertileGrassTile;
        tileBases[(int)SoilType.ForestGrass] = forestGrassTile;
        tileBases[(int)SoilType.PlainsGrass] = plainsGrassTile;
        return tileBases;

    }

    public bool CoordsBelongsToWorld(int x, int y)
    {
        return x >= 0 && x < worldData.MapWidth && y > 0 && y < worldData.MapHeight;
    }

    #endregion
}
