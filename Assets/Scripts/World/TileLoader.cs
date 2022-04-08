using System;
using System.Collections.Generic;
using UnityEngine;

public class TileLoader : MonoBehaviour
{
    public static TileLoader Instance;

    private void Awake()
    {
        Instance = this;
    }


    [SerializeField]
    private Transform playerTransform;
    [Range(1,50)]
    public int viewRangeX;
    [Range(1,50)]
    public int viewRangeY;

    public TileLoadingMode mode;

    public enum TileLoadingMode
    {
        Everything,
        OnlyInteractables
    }

    public List<WorldTile> TilesBeenEverLoaded { get; } = new();
    
    public TileCache TileCache { get; } = new TileCache(0);
    private readonly List<Vector2Int> _loadedTiles = new();
    private WorldData _worldData;
    private WorldManager _worldManager;

    
    private void Start()
    {
        _worldManager = WorldManager.Instance;
        _worldData = _worldManager.WorldData;

        
        TileCache.SetMaxSize(_worldManager.playerSettings.tileCacheSize);
        if (_worldData is null)
        {
            Debug.LogError("TileLoader активен, но мир не сгенерирован");
            return;
        }
        
        // Если мод стоит только на конроль прогрузки объектов, рисует весь мир полностью
        if (mode is TileLoadingMode.OnlyInteractables)
        {
            _worldManager.DrawAllTiles();
        }
    }
    
    private void Update()
    {
        // Прогрузка тайлов вокруг игрока
        Vector3Int playerPosition = Vector3Int.FloorToInt(playerTransform.position);
        for (int x = - viewRangeX; x <= viewRangeX; x++)
        {
            for (int y = - viewRangeY; y <= viewRangeY; y++)
            {
                int targetX = x + playerPosition.x;
                int targetY = y + playerPosition.y;
                // Проверка на выход за пределы карты
                if (!_worldManager.CoordsBelongsToWorld(targetX, targetY)) continue;
                
                // Если тайл еще не загружен, загружает его
                WorldTile tile = _worldData.GetTile(targetX, targetY);
                if (tile.loaded) continue;
                LoadTile(targetX, targetY);
            }
        }

        // Составляет список координат тайлов, которы нужно убрать
        // Из поля зрения игрока
        List<Vector2Int> toRemove = new();
        _loadedTiles.ForEach(tile =>
        {
            Vector2Int target = (Vector2Int) playerPosition - tile;
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

    private void RemoveTile(int x, int y)
    {
        if(mode is TileLoadingMode.Everything) _worldManager.EraseTile(x, y);

        WorldTile tile = _worldData.GetTile(x, y);
        if (tile.HasInteractable)
            TileCache.Add(tile);
        tile.loaded = false;
        // Убирать из loadedTiles не надо, тк это происходит в цикле апдейта
    }

    public void LoadTile(int x, int y)
    {
        if(mode is TileLoadingMode.Everything) _worldManager.DrawTile(x, y);
        
        WorldTile tile = _worldData.GetTile(x, y);
        tile.Load();
        TileCache.Remove(tile);
        _loadedTiles.Add(new Vector2Int(x, y));
        if(!TilesBeenEverLoaded.Contains(tile)) TilesBeenEverLoaded.Add(tile);
    }
    
}