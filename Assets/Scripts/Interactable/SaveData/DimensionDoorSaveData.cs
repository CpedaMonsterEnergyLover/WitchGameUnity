 using UnityEngine;

 public class DimensionDoorSaveData : InteractableSaveData
{
    public WorldScene sceneToLoad;
    public int subWorldIndex;
    public Vector2 position;
    
    public DimensionDoorSaveData(InteractableData origin) : base(origin) { }
    public DimensionDoorSaveData() { }
    public override InteractableSaveData DeepClone()
    {
        return new DimensionDoorSaveData
        {
            id = id,
            creationTime = creationTime,
            initialized = initialized,
            sceneToLoad = sceneToLoad,
            subWorldIndex = subWorldIndex,
            position = position
        };

    }
}
