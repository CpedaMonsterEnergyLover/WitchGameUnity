using UnityEngine;

[System.Serializable]
public class EntitySaveData
{
    [Header("Entity data")] 
    public string id;
    public Vector2 position;
    public bool preInitialised;
    
    // Пустой конструктор для создания сущностей с измененной датой
    protected EntitySaveData() { }

    
    public virtual EntitySaveData DeepClone()
    {
        Debug.LogError("Tried to clone entity data base class.");
        return null;
    }
    
    public EntitySaveData(EntityData origin)
    {
        id = origin.id;
    }
}
