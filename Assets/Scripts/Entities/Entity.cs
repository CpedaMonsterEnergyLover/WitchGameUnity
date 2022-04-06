using System;
using System.Collections;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    // Public fields
    public EntityData Data => data;
    public EntitySaveData SaveData => saveData;

    // Private fields

    // Содержит сохраняемые поля объекта
    [SerializeReference, Header("Instance data")]
    protected EntitySaveData saveData;

    // Содержит общие поля объекта
    [SerializeReference, Header("Data")] protected EntityData data;

    private Transform _playerTransform;
    private WorldManager _worldManager;

    protected float DistanceFromPlayer => 
        ((Vector2)(_playerTransform.position - transform.position)).sqrMagnitude;

    
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
            Vector2Int tilePosition = TilePosition();
            bool isTileLoaded = _worldManager.WorldData.GetTile(tilePosition.x, tilePosition.y).loaded;
        
        
            if (!WorldManager.Instance.CoordsBelongsToWorld(tilePosition))
            {
                Kill();
            }
            else
            {
                if (!isTileLoaded)
                {
                    Despawn();
                }            
            }
            
            yield return new WaitForSeconds(_worldManager.playerSettings.entityDespawnRate);
        }

    }

    public void Despawn()
    {
        Vector2Int tilePosition = TilePosition();
        WorldTile tile = WorldManager.Instance.WorldData.GetTile(tilePosition.x, tilePosition.y);
        tile.entities.Add(this);
        gameObject.SetActive(false); 
    }



    public Vector2Int TilePosition()
    {
        return Vector2Int.FloorToInt(transform.position);
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
