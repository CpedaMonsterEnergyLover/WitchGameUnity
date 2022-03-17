public class CropBed : Interactable, IIgnoreTileRandomisation
{
    public new CropBedSaveData SaveData => (CropBedSaveData) saveData;

    protected override void InitSaveData(InteractableData origin)
    {
        saveData = new CropBedSaveData(origin);
    }

}

public class CropBedSaveData : InteractableSaveData
{
    // Для начальной инициализации
    public CropBedSaveData(InteractableData origin) : base(origin)
    { }

    // Конструктор для клонирования
    private CropBedSaveData()
    { }

    public override InteractableSaveData DeepClone()
    {
        return new CropBedSaveData
        {
            id = id,
            creationHour = creationHour
        };
    }
}

