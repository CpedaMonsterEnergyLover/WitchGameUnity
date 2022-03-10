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
    [SerializeReference, Header("Data")]
    protected EntityData data;

    protected Transform PlayerTransform;

    [SerializeField]
    protected float distanceFromPlayer;

    private void Update()
    {
        Vector2Int tilePosition = TilePosition();

        if (!WorldManager.Instance.CoordsBelongsToWorld(tilePosition))
        {
            Kill();
            Debug.Log($"Сущность {data.name} вышла за пределы мира");
            return;
        }
        
        distanceFromPlayer = ((Vector2)(PlayerTransform.position - transform.position)).sqrMagnitude;
        
        if (distanceFromPlayer > 16.0f * 16.0f)
        {
            WorldTile tile = WorldManager.Instance.WorldData.GetTile(tilePosition.x, tilePosition.y);
            tile.entities.Add(this);
            gameObject.SetActive(false);
        }
    }

    protected virtual void Start()
    {
        PlayerTransform = WorldManager.Instance.playerTransform;
        transform.position = saveData.position;
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
