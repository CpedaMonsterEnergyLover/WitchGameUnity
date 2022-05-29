using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TileLoading;
using UnityEngine;

public class TileLoader : MonoBehaviour
{
    #region Singleton

    public static TileLoader Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion
    
    public enum TileLoadingMode
    {
        Default, // Loads interactables and draws tiles around player
        OnlyInteractables, // Draws all world tiles instantly, loads interactables around player
        Everything, // Draws all tiles and all interactables
    }


    [SerializeField] private Transform playerTransform;
    [Header("Default is 14"), Range(1, 50)] public int viewRangeX;
    [Header("Default is 10"), Range(1, 50)] public int viewRangeY;
    public TileLoadingMode mode;
    private List<WorldTile> _loadedTiles = new();
    private WorldData _worldData;
    private WorldManager _worldManager;

    private Cache<WorldTile> TileCache { get; set; }

    private void ResetPreviousPosition()  => PreviousPlayerPosition = new Vector3Int(int.MaxValue, int.MaxValue, 0);
    private Vector3Int PreviousPlayerPosition { get; set; }
    
    private void Start()
    {
        enabled = false;
        ResetPreviousPosition();
        WorldManager.ONWorldLoaded += ActivateAfterWorldLoad;
    }

    private void OnDestroy()
    {
        WorldManager.ONWorldLoaded -= ActivateAfterWorldLoad;
    }

    private void ActivateAfterWorldLoad()
    {
        playerTransform = PlayerManager.Instance.Transform;
        _worldManager = WorldManager.Instance;
        
        enabled = true;
        _worldData = _worldManager.WorldData;
        TileCache = new Cache<WorldTile>(WorldManager.Instance.playerSettings.tileCacheSize);

        if (_worldData is null)
            throw new Exception("TileLoader активен, но мир не сгенерирован");

        if (mode is TileLoadingMode.Everything)
            LoadEverything();
        else if (mode is TileLoadingMode.OnlyInteractables) 
            _worldManager.DrawAllTiles();
    }

    private void LoadEverything()
    {
        for (var x = 0; x < _worldData.MapWidth; x++)
        for (var y = 0; y < _worldData.MapHeight; y++)
            LoadTile(_worldData.GetTile(x, y));
        enabled = false;
    }
    
    private void FixedUpdate()
    {
        if (_worldData is null)
        {
            Debug.Log("WorldData is null");
            return;
        }
        var playerPosition = Vector3Int.FloorToInt(playerTransform.position);
        if(playerPosition.Equals(PreviousPlayerPosition)) return;
        
        PreviousPlayerPosition = playerPosition;
        for (var x = -viewRangeX; x <= viewRangeX; x++)
        for (var y = -viewRangeY; y <= viewRangeY; y++)
        {
            var targetX = x + playerPosition.x;
            var targetY = y + playerPosition.y;

            // Если тайл еще не загружен, загружает его
            var tile = _worldData.GetTile(targetX, targetY);
            if(tile is null || tile.IsLoaded) continue;
            LoadTile(tile);
        }

        // Составляет список координат тайлов, которы нужно убрать
        // Из поля зрения игрока
        List<WorldTile> toRemove = new();
        foreach (var tile in _loadedTiles)
        {
            Vector2Int target =  (Vector2Int) playerPosition - tile.Position;
            if (Math.Abs(target.x) > viewRangeX || Math.Abs(target.y) > viewRangeY)
                toRemove.Add(tile);
        }

        // Очищает все тайлы из составленного списка
        foreach (var tile in toRemove)
        {
            RemoveTile(tile);
            _loadedTiles.Remove(tile);
        }

        toRemove.Clear();
    }

    public void UnloadAndBlock(int x, int y)
    {
        WorldTile tile = _worldManager.WorldData.GetTile(x, y);
        if(tile is null || tile.IsBlockedForLoading) return;
        RemoveTile(tile);
        tile.IsBlockedForLoading = true;
    }
    
    private void RemoveTile(WorldTile tile)
    {
        if(tile.IsBlockedForLoading || !tile.IsLoaded) return;
        if (mode is TileLoadingMode.Default) 
            _worldManager.EraseTile(new Vector3Int(tile.Position.x, tile.Position.y, 0));

        if (tile.HasInteractable)
            TileCache.Add(tile);
        tile.IsLoaded = false;
        tile.lastLoadedMinute = Timeline.TotalMinutes;
        // Убирать из loadedTiles не надо, тк это происходит в цикле апдейта
    }

    private void LoadTile(WorldTile tile)
    {
        if(tile.IsBlockedForLoading) return;
        if (mode is TileLoadingMode.Default or TileLoadingMode.Everything) 
            _worldManager. DrawTile(tile);

        tile.Load();
        TileCache.Remove(tile);
        _loadedTiles.Add(tile);
    }

    public void Reload()
    {
        foreach (WorldTile tile in _loadedTiles)
        {
            _worldManager.EraseTile(new Vector3Int(tile.Position.x, tile.Position.y, 0));
            tile.OnPopped();
            tile.lastLoadedMinute = Timeline.TotalMinutes;
            tile.IsLoaded = false;
        }
        _loadedTiles = new List<WorldTile>();
        ResetPreviousPosition();
        if (mode is TileLoadingMode.Everything)
            LoadEverything();
        else if (mode is TileLoadingMode.OnlyInteractables) 
            _worldManager.DrawAllTiles();
    }
    
}