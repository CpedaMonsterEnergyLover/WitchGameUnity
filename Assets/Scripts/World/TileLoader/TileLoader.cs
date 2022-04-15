﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        Everything,
        OnlyInteractables
    }


    [SerializeField] private Transform playerTransform;
    [Header("Default is 14"), Range(1, 50)] public int viewRangeX;
    [Header("Default is 10"), Range(1, 50)] public int viewRangeY;
    public TileLoadingMode mode;
    private List<WorldTile> _loadedTiles = new();
    private WorldData _worldData;
    private WorldManager _worldManager;

    public Cache<WorldTile> TileCache { get; private set; }

    private void Start()
    {
        
        playerTransform = FindObjectOfType<PlayerController>().transform;
        _worldManager = WorldManager.Instance;
        _worldData = _worldManager.WorldData;
        TileCache = new Cache<WorldTile>(WorldManager.Instance.playerSettings.tileCacheSize);

        if (_worldData is null)
        {
            Debug.LogError("TileLoader активен, но мир не сгенерирован");
            return;
        }

        // Если мод стоит только на конроль прогрузки объектов, рисует весь мир полностью на старте
        if (mode is TileLoadingMode.OnlyInteractables) _worldManager.DrawAllTiles();
    }

    private void FixedUpdate()
    {
        // Прогрузка тайлов вокруг игрока
        var playerPosition = Vector3Int.FloorToInt(playerTransform.position);
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

    private void RemoveTile(WorldTile tile)
    {
        if (mode is TileLoadingMode.Everything) 
            _worldManager.EraseTile(new Vector3Int(tile.Position.x, tile.Position.y, 0));

        if (tile.HasInteractable)
            TileCache.Add(tile);
        tile.IsLoaded = false;
        // Убирать из loadedTiles не надо, тк это происходит в цикле апдейта
    }

    private void LoadTile(WorldTile tile)
    {
        if (mode is TileLoadingMode.Everything) 
            _worldManager.DrawTile(tile.Position.x, tile.Position.y);

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
            tile.IsLoaded = false;
        }
        _loadedTiles = new();
    }

    public async Task<bool> AwaitAllTilesLoaded()
    {
        int tilesToAwait = viewRangeX * viewRangeY;
        while (_loadedTiles.Count < tilesToAwait)
        {
            await Task.Delay((int) Time.deltaTime * 1000);
        }

        return true;
    }
    

}