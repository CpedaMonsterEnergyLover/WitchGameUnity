using System.Collections.Generic;
using UnityEngine;


// Хранит информацию о тайле мира
[System.Serializable]
public class WorldTile
{
    public Interactable instantiatedInteractable;
    public InteractableSaveData savedData;
    public bool loaded;
    public bool cached;
    public Vector2 interactableOffset;
    public List<Entity> entities = new();

    public bool[] Layers { get; private set; }
    public Vector2Int Position { get; private set; }
    
    public bool HasInteractable => savedData is not null;


    public WorldTile(int x, int y, bool[] tiles, InteractableData interactableData)
    {
        Position = new Vector2Int(x, y);
        Layers = tiles;
        savedData = interactableData is null ? null : 
            new InteractableSaveData(interactableData);
        interactableOffset =  new Vector2(Random.value * 0.6f + 0.2f, Random.value * 0.6f + 0.2f);
    }
    
    
    // Загружает данные клетки на сцену
    public void Load()
    {
        loaded = true;
        LoadEntities();
        LoadInteractable();
    }

    public void LoadInteractable()
    {
        if (!HasInteractable) return;
        if (!GameCollection.Interactables.ContainsID(savedData.id))
        {
            Debug.LogWarning($"Попытка загрузки Interactable с несуществующим айди {savedData.id}");
            savedData = null;
            return;
        }

        if (cached)
            HideInteractable(false);
        else
            instantiatedInteractable = Interactable.Create(saveData: savedData);

        instantiatedInteractable.transform.position = instantiatedInteractable.Data.ignoreOffset ?
            new Vector3(Position.x + 0.5f, Position.y + 0.5f, 0)
            : new Vector3(Position.x + interactableOffset.x, Position.y + interactableOffset.y, 0);
        instantiatedInteractable.OnTileLoad(this);

    }

    public void LoadEntities()
    {
        entities.ForEach(entity => entity.gameObject.SetActive(true));
        entities.Clear();
    }

    
    // Убирает объект interactable этого тайла из мира
    public void UnloadInteractable()
    {
        savedData = instantiatedInteractable.SaveData.DeepClone();
        Object.DestroyImmediate(instantiatedInteractable.gameObject);
        instantiatedInteractable = null;
    }

    public void DestroyInteractable()
    {
        instantiatedInteractable.Destroy();
    }


    public void HideInteractable(bool isHidden)
    {
        if(instantiatedInteractable is not null) instantiatedInteractable.SetActive(!isHidden);
    }
    
}
