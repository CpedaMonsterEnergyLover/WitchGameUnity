using System;
using UnityEngine;

// Хранит информацию о тайлах мира
// То есть исключительно о слоях грида 
// НЕ сущности и НЕ объекты
[System.Serializable]
public class WorldTile
{
    public SoilType[] layers = {SoilType.None, SoilType.None, SoilType.None, SoilType.None};
    public float moistureLevel;
    public InteractableSaveData interactableSaveData;
    public GameObject instantiatedObject;
    public bool loaded;
    public bool cached;
    public Vector3Int position;

    public void InstantiateInteractable(Transform attachTo)
    {
        // Если объект загружается, он удаляется из кеша 
        loaded = true;
        cached = false;
        if (interactableSaveData is null)
        {
            instantiatedObject = null;
            return;
        }

        GameObject prefab = InteractableObjects.Collection
            [(int) interactableSaveData.interactableType][interactableSaveData.interactableID].prefab;
        instantiatedObject = Interactable.Create(prefab, attachTo, interactableSaveData);
    }
    
    public void AddLayer(GridLayer layer, SoilType soilType)
    {
        layers[(int) layer] = soilType;
    }

    public SoilType GetLayer(GridLayer layer)
    {
        return layers[(int) layer];
    }
    
    public void RemoveLayer(GridLayer layer,  SoilType soilType)
    {
        layers[(int) layer] = soilType;
    }
}

public enum SoilType
{
    Water,
    Swamp,
    Sand,
    FertileGrass,
    ForestGrass,
    PlainsGrass,
    None
}

public enum GridLayer
{
    Ground,
    Plains,
    Sand,
    Water
}
