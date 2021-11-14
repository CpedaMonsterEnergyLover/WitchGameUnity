using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    #region Vars

    // Public
    [Header("Настройки")] 
    public int chunksCacheSize = 100;
    public int viewRangeX = 3;
    public int viewRangeY = 3;
    public int chunkSize = 10;
    
    [Header("Игрок (GameObject)")]
    public GameObject player;
    public GameObject chunkPrefab;
    
    private static Dictionary<Vector2, Chunk> _chunks = new Dictionary<Vector2, Chunk>();

    #endregion



    #region UnityMethods

    private void Update()
    {
        Vector2 playerChunkPosition = GetPlayerChunkPosition();
        
        Dictionary<Vector2, Chunk> toRemove = new Dictionary<Vector2, Chunk>();
        
        foreach (var chunk in _chunks.Values)
        {
            if(chunk.TooFarFromPlayer(playerChunkPosition, viewRangeX, viewRangeY)) toRemove[chunk.cords] = chunk;
        }

        foreach (var chunkKey in toRemove.Keys)
        {
            _chunks.Remove(chunkKey);
        }
        
        LoadChunksAroundPlayer(playerChunkPosition, viewRangeX, viewRangeY);
    }

    private void Start()
    {
        player.transform.position = new Vector3(chunkSize / 2f, chunkSize / 2f, 0);
    }

    #endregion



    #region ClassMethods

    private void LoadChunksAroundPlayer(Vector2 from, int distanceX, int distanceY)
    {
        for (int x = -distanceX + 1; x < distanceX; x++)
        {
            for (int y = -distanceY + 1; y < distanceY; y++)
            {
                Vector2 pos = new Vector2(x, y) + from;
                if (!_chunks.ContainsKey(pos))
                {
                    Chunk chunk = new Chunk(chunkPrefab, transform, chunkSize, pos);
                    _chunks[pos] = chunk;
                }
            }
        }
    }

    #endregion



    #region UtilMethods

    public Vector2 GetPlayerChunkPosition()
    {
        Vector3 playerPosition = player.transform.position;
        return Chunk.Of(playerPosition.x, playerPosition.y, chunkSize);
    }

    #endregion
}
