using UnityEngine;


// Хранит информацию о тайле мира
[System.Serializable]
public class WorldTile
{
    public Interactable instantiatedInteractable;
    public InteractableSaveData savedData;
    public bool loaded;
    public bool cached;
    public Vector2 interactableOffset = Vector2.zero;

    public bool[] Layers { get; private set; }
    public Vector2Int Position { get; private set; }
    
    public bool HasInteractable => savedData is not null;

    public WorldTile(){}

    public WorldTile(int x, int y, bool[] tiles)
    {
        Position = new Vector2Int(x, y);
        Layers = tiles;
    }
    
    // Загружает данные клетки на сцену

    public void Load()
    {
        loaded = true;
       
        if (!HasInteractable) return;
        LoadInteractable();
    }

    public void LoadInteractable()
    {
        if (!GameCollection.Interactables.ContainsID(savedData.id))
        {
            Debug.LogWarning($"The given key was not present in the Collection of objects: {savedData.id}");
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
