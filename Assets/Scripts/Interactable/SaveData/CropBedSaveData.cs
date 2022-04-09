[System.Serializable]
public class CropBedSaveData : InteractableSaveData
{
    public CropBedSaveData(InteractableData origin) : base(origin){ }
    private CropBedSaveData(){ }
    public override InteractableSaveData DeepClone()
    {
        return new CropBedSaveData
        {
            id = id,
            creationHour = creationHour,
            initialized = initialized
        };
    }
}