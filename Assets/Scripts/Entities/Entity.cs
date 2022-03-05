using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    // Public fields
    public EntityData Data => data;
    public EntitySaveData InstanceData => instanceData;
    
    // Private fields
    
    // Содержит сохраняемые поля объекта
    [SerializeReference, Header("Instance data")]
    protected EntitySaveData instanceData;
    // Содержит общие поля объекта
    [SerializeReference, Header("Data")]
    protected EntityData data;

    


    public virtual void Kill()
    {
        
    }
    
        
    // Creates a new entity
    public static Entity Create(EntitySaveData saveData)
    {
        return null;
    }
}
