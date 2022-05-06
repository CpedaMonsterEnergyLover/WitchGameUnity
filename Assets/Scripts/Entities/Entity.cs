using System.Collections;
using TileLoading;
using UnityEngine;

public abstract class Entity : MonoBehaviour, ICacheable
{
    public EntityData Data => data;
    public EntitySaveData SaveData =>  (ItemEntitySaveData) saveData;

    [SerializeReference, Header("Instance data")]
    protected EntitySaveData saveData;

    [SerializeReference, Header("Data")] 
    protected EntityData data;

    private Transform _playerTransform;
    private static WorldManager WorldManager => WorldManager.Instance;
    private WorldTile _dataHandlerTile;

    
    protected float DistanceFromPlayer => 
        Vector2.Distance(_playerTransform.position, transform.position);

    
    #region ICacheable

    public bool IsLoaded { get; set; }
    public bool IsCached { get; set; }
    public GameObject GetCacheableItem => gameObject;

    public void OnPopped()
    {
        if(SaveOnTile(out WorldTile tile))   
            tile.RemoveEntityFromCache(this);
        else DestroyImmediate(gameObject);
    }
    
    #endregion

    public bool SaveOnTile(out WorldTile tile)
    {
        if(GetWorldTilePosition(out WorldTile _tile))
        {
            saveData.position = transform.position;
            _tile.AddEntitySaveData(saveData.DeepClone()); 
        }
        tile = _tile;
        return tile is not null;
    }
    
    protected virtual void Start()
    {
        _playerTransform = PlayerManager.Instance.Transform;
        Load();
    }

    private void OnDestroy()
    {
        WorldManager.RemoveEntity(this);
        _dataHandlerTile?.RemoveEntitySaveData(saveData);
        WorldManager.RemoveEntityFromCache(this);
    }

    public void Load()
    {
        WorldManager.AddEntity(this);
        WorldManager.RemoveEntityFromCache(this);
        transform.position = saveData.position;
        gameObject.SetActive(true);
        StartCoroutine(DespawnCoroutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator DespawnCoroutine()
    {
        while (gameObject.activeInHierarchy)
        {
            if (!GetWorldTilePosition(out WorldTile tile))
            {
                Kill();
            }
            else
            {
                if (!tile.IsLoaded)
                {
                    Despawn();
                }            
            }
            
            yield return new WaitForSeconds(WorldManager.playerSettings.entityDespawnRate);
        }

    }

    private void Despawn()
    {
        if (!GetWorldTilePosition(out WorldTile tile))
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            saveData.position = transform.position;
            WorldManager.CacheEntity(this);
            tile.CacheEntity(this);
            _dataHandlerTile = tile;
        }
    }
    
    
    
    
    // Creates a new entity
    public static Entity Create(EntitySaveData saveData)
    {
        GameObject prefab = GameCollection.Entities.Get(saveData.id);
        prefab = Instantiate(prefab, WorldManager.Instance.entitiesTransform);
        Entity interactable = prefab.GetComponent<Entity>();

        if (saveData.preInitialised)
        {
            interactable.saveData = saveData;
        }
        else interactable.InitSaveData(interactable.Data);
        
        return interactable;
    }

    public virtual void Kill()
    {
        Destroy(gameObject);
    }
    
    protected virtual void InitSaveData(EntityData origin)
    { }
    
    private bool GetWorldTilePosition(out WorldTile tile)
    {
        var floorPos =  Vector2Int.FloorToInt(transform.position);
        tile = WorldManager.Instance.WorldData.GetTile(floorPos.x, floorPos.y);
        return tile is not null;
    } 
    
}
