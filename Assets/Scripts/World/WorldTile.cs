using UnityEngine;
using UnityEngine.Tilemaps;


// Хранит информацию о тайле мира\
[System.Serializable]
public class WorldTile
{
    public SoilType[] layers = {SoilType.None, SoilType.None, SoilType.None, SoilType.None};
    public float moistureLevel;
    public bool HasInteractable => savedData is not null;
    public Interactable instantiatedInteractable;
    public InteractableSaveData savedData;
    public bool loaded;
    public bool cached;
    public Vector3Int position;

    // Загружает данные клетки на сцену
    public void Load(Tilemap[] tilemaps, TileBase[] tilebases)
    {
        loaded = true;

        // Рисует слои грида
        Draw(tilemaps, tilebases);
        
        if (!HasInteractable) return;
        LoadInteractable();
    }

    public Interactable LoadInteractable()
    {
        if (!GameObjectsCollection.InteractableCollection.ContainsKey(savedData.identifier.id))
        {
            Debug.LogWarning($"The given key was not present in the Collection of objects:{savedData.identifier.id}");
            savedData = null;
            return null;
        }

        if (cached)
        {
            SetHidden(false);
        }
        else
        {
            instantiatedInteractable = Interactable.Create(savedData);

        }

        instantiatedInteractable.transform.position =
            new Vector3(position.x + 0.5f, position.y + 0.5f, 0);
        instantiatedInteractable.OnTileLoad(this);

        return instantiatedInteractable;
    }

    // Помещает слои тайла на слои грида
    public void Draw(Tilemap[] tilemaps, TileBase[] tilebases)
    {
        for (int i = 0; i < layers.Length; i++)
        {
            // Если на слое есть почва, ставит ее на грид
            if (layers[i] != SoilType.None)
                tilemaps[i].SetTile(position, tilebases[(int) layers[i]]);
        }
    }
    
    // Убирает объект interactable этого тайла из мира
    public void UnloadInteractable()
    {
        savedData = instantiatedInteractable.InstanceData.DeepClone();
        Object.DestroyImmediate(instantiatedInteractable.gameObject);
        instantiatedInteractable = null;
    }

    public void DestroyInteractable()
    {
        instantiatedInteractable.Destroy();
    }

    // Убирает слои тайла с грида
    public void Erase(Tilemap[] tilemaps)
    {
        // Проходит по всем слоям
        for (int i = 0; i < layers.Length; i++)
        {
            /*// Если на нем есть что-то, чистит тайл
            if (layers[i] != SoilType.None)*/
                tilemaps[i].SetTile(position, null);
        }
    }

    public void SetHidden(bool isHidden)
    {
        if(instantiatedInteractable is not null) instantiatedInteractable.SetActive(!isHidden);
    }
    
    

    #region Utils

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

    #endregion
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
