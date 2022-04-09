public class CropBed : Interactable, IIgnoreTileRandomisation
{
    public new CropBedSaveData SaveData => (CropBedSaveData) saveData;

    protected override void InitSaveData(InteractableData origin)
    {
        saveData = new CropBedSaveData(origin) { initialized = true };
    }

}



