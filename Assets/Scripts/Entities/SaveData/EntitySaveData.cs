using UnityEngine;

[System.Serializable]
public abstract class EntitySaveData
{
    [Header("Entity data")]
    public EntityIdentifier identifier;
    public Vector2 position;
    
    // Пустой конструктор для создания сущностей с измененной датой
    protected EntitySaveData() { }

    
    public virtual EntitySaveData DeepClone()
    {
        Debug.LogError("Tried to clone entity data base class.");
        return null;
    }
}
