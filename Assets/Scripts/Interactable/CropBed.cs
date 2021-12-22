public class CropBed : Interactable
{
    public new CropBedSaveData InstanceData => (CropBedSaveData) instanceData;

    protected override void InitInstanceData(InteractableSaveData saveData)
    {
        instanceData = new CropBedSaveData(saveData);
        base.InitInstanceData(saveData);
    }

    public override void Interact(float value = 1)
    {
    }
}

public class CropBedSaveData : InteractableSaveData
{
    // Для инициализации значеий
    public CropBedSaveData(InteractableSaveData interactableSaveData)
    {
        creationHour = interactableSaveData.creationHour;
        instanceID = interactableSaveData.instanceID;
        identifier = interactableSaveData.identifier;
    }

    // Нужен для клонирования
    private CropBedSaveData()
    {
    }

    public override InteractableSaveData DeepClone()
    {
        return new CropBedSaveData
        {
            instanceID = instanceID,
            identifier = identifier,
            creationHour = creationHour,
            
            
        };
    }
}

