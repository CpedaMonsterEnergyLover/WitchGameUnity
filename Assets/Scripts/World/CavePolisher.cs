using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class CavePolisher : MonoBehaviour
{
    public Transform gridTransform;
    public Sprite polishingSprite;
    public TileBase wallTileBase;
    public TileBase polishingTileBase;
    public TileBase temporaryTileBase;
    public WorldLayer polishingLayer;

    private GameObject _hollowsTileMapGO;
    private CaveEntrance _caveEntrance;
    private readonly List<Vector3Int> _removedTiles = new();

    public class CaveEntrance
    {
        public enum EntranceType
        {
            Wall,
            Floor
        }
        
        public CaveEntrance refersTo;
        public Vector3Int position;
        public EntranceType type;

        public CaveEntrance(CaveEntrance refersTo, Vector3Int position, EntranceType type)
        {
            this.refersTo = refersTo;
            this.position = position;
            this.type = type;
        }
    }

    private List<List<Vector3Int>> GetHollowZones(WorldData worldData)
    {
        // Подготовка слоя грида для полировки чтобы не писать свой Flood :)
        if(_hollowsTileMapGO is not null) DestroyImmediate(_hollowsTileMapGO);
        _hollowsTileMapGO = new GameObject("Hollows");
        Tilemap hollowsTilemap = _hollowsTileMapGO.AddComponent<Tilemap>();
        // _hollowsTileMapGO.AddComponent<TilemapRenderer>();
        _hollowsTileMapGO.transform.SetParent(gridTransform);

        List<Vector3Int> hollowTiles = GetTiles(worldData, polishingLayer.tilemap, null);
        foreach (Vector3Int tile in hollowTiles) hollowsTilemap.SetTile(tile, temporaryTileBase);


        List<List<Vector3Int>> hollowZones = new List<List<Vector3Int>>();
        
        // Определение отдельных пустот пещеры
        while (hollowTiles.Count > 0)
        {
            hollowsTilemap.FloodFill(hollowTiles[0], polishingTileBase);
            
            List<Vector3Int> hollowZone = new();
            hollowTiles.ForEach(pos =>
            {
                if (hollowsTilemap.GetTile(pos) == polishingTileBase)
                {
                    hollowsTilemap.SetTile(pos, temporaryTileBase);
                    hollowZone.Add(pos);
                }
            });
            
            hollowZone.ForEach(temp => hollowTiles.Remove(temp));
            
            hollowZones.Add(hollowZone);
        }
        
        
        
        DestroyImmediate(_hollowsTileMapGO);

        return hollowZones;
    }

    private void ClearHollows(WorldData worldData)
    {
        _removedTiles.ForEach(i => worldData.GetTile(i.x, i.y).SetInteractable(null));
    }
    
    public List<Vector3Int> GetMainHollow(WorldData worldData)
    {
        var hollowZones = GetHollowZones(worldData).OrderByDescending(ints => ints.Count).ToList();
        var maxHollow = hollowZones.First();

        int layerIndex = polishingLayer.index;
        var polishingLayerTilemap = polishingLayer.tilemap;
        
        
        while (hollowZones.Count > 1)
        {
            List<Vector3Int> zoneToRemove = hollowZones[1];
            
            foreach (Vector3Int i in zoneToRemove)
            {
                polishingLayerTilemap.SetTile(i, wallTileBase);
                worldData.GetTile(i.x, i.y).SetLayer(layerIndex, true);

                _removedTiles.Add(i);
            }
            hollowZones.RemoveAt(1);
        }

        ClearHollows(worldData);
        
        return maxHollow;
    }
    
    public CaveEntrance GetCaveEntrance(List<Vector3Int> hollowZone)
    {
        var suitLocations = hollowZone
            .Where(i => !hollowZone.Contains(i + new Vector3Int(0, 1, 0)) &&
                        !hollowZone.Contains(i + new Vector3Int(-1, 1, 0)) &&
                        !hollowZone.Contains(i + new Vector3Int(1, 1, 0)))
            .ToList();

        if(suitLocations.Count == 0) 
            _caveEntrance = new CaveEntrance(
            null, 
            hollowZone[Random.Range(0, hollowZone.Count)], 
            CaveEntrance.EntranceType.Floor);
        else
            _caveEntrance = new CaveEntrance(
            null, 
            suitLocations[Random.Range(0, suitLocations.Count)], 
            CaveEntrance.EntranceType.Wall);
        
        return _caveEntrance;
    }

    private List<Vector3Int> GetTiles(WorldData worldData, Tilemap tilemap, TileBase tileBase)
    {
        List<Vector3Int> hollowsList = new();

        for(int x = 0; x < worldData.MapWidth; x++)
        for (int y = 0; y < worldData.MapHeight; y++)
        {
            Vector3Int pos = new Vector3Int(x, y, 0);
            if (tilemap.GetTile(pos) == tileBase) hollowsList.Add(pos);
        }

        return hollowsList;
    }

    
    public void PolishWalls(WorldData worldData)
    {
        while (PolishLayer(worldData) > 0)
        {
            PolishLayer(worldData);
        } 
    }

    private int PolishLayer(WorldData worldData)
    {
        int counter = 0;
        int layerIndex = polishingLayer.index;
        var polishingLayerTilemap = polishingLayer.tilemap;

        
        for(int x = 0; x < worldData.MapWidth; x++)
        for (int y = 0; y < worldData.MapHeight; y++)
        {
            Vector3Int pos = new Vector3Int(x, y, 0);
            if (polishingLayerTilemap.GetSprite(pos) == polishingSprite)
            {
                polishingLayerTilemap.SetTile(pos, null);

                worldData.GetTile(x, y).SetLayer(layerIndex, false);

                counter++;
            }
        }
        return counter;
    }


    private void OnDrawGizmos()
    {
        if(_caveEntrance is null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_caveEntrance.position + new Vector3(0.5f, 0.5f, 0), 0.25f);
    }
}
