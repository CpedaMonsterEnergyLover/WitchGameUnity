using UnityEngine;

public class CropBed : Interactable, IIgnoreTileRandomisation
{
    public new CropBedSaveData SaveData => (CropBedSaveData) saveData;
    
    protected override void InitSaveData(InteractableData origin)
    {
        saveData = ScriptableObject.CreateInstance<CropBedSaveData>();
        saveData.id = origin.id;
    }
}


