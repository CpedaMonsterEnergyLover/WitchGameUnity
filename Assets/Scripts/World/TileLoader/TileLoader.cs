using System;
using System.Collections.Generic;
using TileLoading;
using UnityEngine;

public class TileLoader : MonoBehaviour
{
    public enum TileLoadingMode
    {
        Everything,
        OnlyInteractables
    }


    [SerializeField] private Transform playerTransform;
    [Range(1, 50)] public int viewRangeX;
    [Range(1, 50)] public int viewRangeY;
    public TileLoadingMode mode;
    private readonly List<Vector2Int> _loadedTiles = new();
    private WorldData _worldData;
    private WorldManager _worldManager;

    public Cache<WorldTile> TileCache { get; private set; }
    public Cache<Entity> EntityCache { get; private set; }

    private void Start()
    {
        
        playerTransform = FindObjectOfType<PlayerController>().transform;
        _worldManager = WorldManager.Instance;
        _worldData = _worldManager.WorldData;
        TileCache = new Cache<WorldTile>(WorldManager.Instance.playerSettings.tileCacheSize);
        EntityCache = new Cache<Entity>(WorldManager.Instance.playerSettings.entitiesCacheSize);

        if (_worldData is null)
        {
            Debug.LogError("TileLoader активен, но мир не сгенерирован");
            return;
        }

        // Если мод стоит только на конроль прогрузки объектов, рисует весь мир полностью
        if (mode is TileLoadingMode.OnlyInteractables) _worldManager.DrawAllTiles();
    }

    private void Update()
    {
        // Прогрузка тайлов вокруг игрока
        var playerPosition = Vector3Int.FloorToInt(playerTransform.position);
        for (var x = -viewRangeX; x <= viewRangeX; x++)
        for (var y = -viewRangeY; y <= viewRangeY; y++)
        {
            var targetX = x + playerPosition.x;
            var targetY = y + playerPosition.y;
            // Проверка на выход за пределы карты
            if (!_worldManager.CoordsBelongsToWorld(targetX, targetY)) continue;

            // Если тайл еще не загружен, загружает его
            var tile = _worldData.GetTile(targetX, targetY);
            if (tile.IsLoaded) continue;
            LoadTile(targetX, targetY);
        }

        // Составляет список координат тайлов, которы нужно убрать
        // Из поля зрения игрока
        List<Vector2Int> toRemove = new();
        _loadedTiles.ForEach(tile =>
        {
            var target = (Vector2Int) playerPosition - tile;
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
        if (mode is TileLoadingMode.Everything) _worldManager.EraseTile(x, y);

        var tile = _worldData.GetTile(x, y);
        if (tile.HasInteractable)
            TileCache.Add(tile);
        tile.IsLoaded = false;
        // Убирать из loadedTiles не надо, тк это происходит в цикле апдейта
    }

    private void LoadTile(int x, int y)
    {
        if (mode is TileLoadingMode.Everything) _worldManager.DrawTile(x, y);

        var tile = _worldData.GetTile(x, y);
        tile.Load();
        TileCache.Remove(tile);
        _loadedTiles.Add(new Vector2Int(x, y));
    }

    #region Singleton

    public static TileLoader Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion
}