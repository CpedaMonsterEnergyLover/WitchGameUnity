using System;
using System.Collections;
using TileLoading;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class Entity : MonoBehaviour, ICacheable
{
    public EntityData Data => data;
    public EntitySaveData SaveData => saveData;

    [SerializeReference, Header("Instance data")]
    protected EntitySaveData saveData;

    [SerializeReference, Header("Data")] 
    protected EntityData data;

    private Transform _playerTransform;
    private WorldManager _worldManager;

    private bool GetWorldTilePosition(out WorldTile tile)
    {
        var floorPos =  Vector2Int.FloorToInt(transform.position);
        tile = WorldManager.Instance.WorldData.GetTile(floorPos.x, floorPos.y);
        return tile is not null;
    } 
    
    protected float DistanceFromPlayer => 
        Vector2.Distance(_playerTransform.position, transform.position);

    
    #region ICacheable

    public bool IsLoaded { get; set; }
    public bool IsCached { get; set; }
    public GameObject GetCacheableItem => gameObject;

    public void LeaveCache()
    {
        if(GetWorldTilePosition(out WorldTile tile))
        {
            tile.savedEntities.Add(saveData.DeepClone());
        } 
        DestroyImmediate(gameObject);
    }
    
    #endregion
    
    
    protected virtual void Start()
    {
        _worldManager = WorldManager.Instance;
        _playerTransform = _worldManager.playerTransform;
        transform.position = saveData.position;
        Load();
    }
    
    public void Load()
    {
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
            
            yield return new WaitForSeconds(_worldManager.playerSettings.entityDespawnRate);
        }

    }

    public void Despawn()
    {
        if (!GetWorldTilePosition(out WorldTile tile))
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            tile.CachedEntities.Add(this);
            gameObject.SetActive(false);
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
    
}
