using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BattleArena : MonoBehaviour
{
    public static BattleArena Instance;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }


    public Tilemap mainTilemap;
    public TileBase mainTilebase;
    public Tilemap bgTilemap;
    public TileBase bgTilebase;
    public int arenaRadius;
    public float paintIterationDelay;
    public BattleArenaBorder border;
    
    private TileLoader _tileLoader;
    private WorldData _worldData;
    private Vector3Int _center;
    
    private void Init()
    {
        _worldData = WorldManager.Instance.WorldData;
        _tileLoader = TileLoader.Instance;
        _center = Vector3Int.FloorToInt(transform.position);
    }


    public void ClearTiles()
    {
        StopAllCoroutines();
        mainTilemap.ClearAllTiles();
        bgTilemap.ClearAllTiles();
    }


    private void PaintBackground()
    {
        for(int x = -arenaRadius - 1; x <= arenaRadius; x++)
        for(int y = -arenaRadius - 1; y <= arenaRadius; y++)
            bgTilemap.SetTile(new Vector3Int(x, y, 0), bgTilebase);
    }

    public void ClearArena()
    {
        StartCoroutine(ClearArenaRoutine());
    }

    public void SetPosition(Vector3 newPos) => transform.position = newPos;
    
    public void PaintArena()
    {
        Init();
        gameObject.SetActive(true);
        PaintBackground();
        StartCoroutine(PaintArenaRoutine());
        border.SetRadius(arenaRadius);
        border.SetActive(true);
    }
    
    private IEnumerator PaintArenaRoutine()
    {
        mainTilemap.SetTile(new Vector3Int(0, 0, 0), mainTilebase);
        int currentR = 1;
        while (currentR <= arenaRadius)
        {
            int sections = currentR * 24;
            float angle = 360f / sections;
            float currentAngle = 0;
            Vector3Int alreadyPainted = Vector3Int.zero;
            for (int i = 0; i < sections; i++)
            {
                int x = Mathf.FloorToInt(Mathf.Cos(currentAngle) * currentR);
                int y = Mathf.FloorToInt(Mathf.Sin(currentAngle) * currentR);
                Vector3Int position = new Vector3Int(x, y, 0);
                currentAngle += angle;

                if (alreadyPainted != position)
                {
                    mainTilemap.SetTile(position, mainTilebase);
                    alreadyPainted = position;
                    if (Application.isPlaying && arenaRadius != currentR)
                    {
                        position += _center;
                        _tileLoader.UnloadAndBlock(position.x, position.y); 
                    }
                }                
            }
            currentR++;
            yield return new WaitForSeconds(paintIterationDelay);
        }
    }
    
    private IEnumerator ClearArenaRoutine()
    {
        int currentR = arenaRadius;
        while (currentR >= 1)
        {
            int sections = currentR * 24;
            float angle = 360f / sections;
            float currentAngle = 0;
            Vector3Int alreadyPainted = Vector3Int.zero;
            for (int i = 0; i < sections; i++)
            {
                int x = Mathf.FloorToInt(Mathf.Cos(currentAngle) * currentR);
                int y = Mathf.FloorToInt(Mathf.Sin(currentAngle) * currentR);
                Vector3Int position = new Vector3Int(x, y, 0);
                currentAngle += angle;

                if (alreadyPainted != position)
                {
                    mainTilemap.SetTile(position, null);
                    alreadyPainted = position;
                    if (Application.isPlaying)
                    {
                        position += _center;
                        WorldTile tile = _worldData.GetTile(position.x, position.y);
                        if (tile is not null) tile.IsBlockedForLoading = false;
                        else Debug.Log($"Tile at position {position} is null");
                    }
                }                
            }
            currentR--;
            yield return new WaitForSeconds(paintIterationDelay);
        }
        
        mainTilemap.ClearAllTiles();
        bgTilemap.ClearAllTiles();
        gameObject.SetActive(false);
    }
}
