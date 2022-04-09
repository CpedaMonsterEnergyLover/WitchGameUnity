using System.Collections.Generic;
using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


[Serializable]
public class WorldTile
{
    [SerializeField] public Vector2 interactableOffset;
    [SerializeReference] public InteractableSaveData savedData;
    [SerializeField] private bool[] layers;
    [SerializeField] private Vector2Int position;

    [NonSerialized] public Interactable instantiatedInteractable;
    [NonSerialized] public bool loaded;
    [NonSerialized] public bool cached;
    // TODO: manage this 
    [NonSerialized] public List<Entity> entities = new();

    public bool[] Layers => layers;
    public Vector2Int Position
    {
        get => position;
        set => position = value;
    }
    public bool HasInteractable => savedData is not null;


    public WorldTile(int x, int y, bool[] tiles, InteractableData interactableData)
    {
        Position = new Vector2Int(x, y);
        layers = tiles;
        savedData = interactableData is null ? null : 
            new InteractableSaveData(interactableData);
        interactableOffset =  new Vector2(Random.value * 0.6f + 0.2f, Random.value * 0.6f + 0.2f);
        // mirrored = Random.Range(0, 2) == 1;
    }

    public void SetLayer(int layerIndex, bool value)
    {
        Layers[layerIndex] = value;
    }

    public WorldTile SetData(WorldTile from)
    {
        interactableOffset = from.interactableOffset;
        savedData = from.savedData;
        layers = from.Layers;
        position = from.Position;
        return this;
    }
    
    
    // Загружает данные клетки на сцену
    public void Load()
    {
        loaded = true;
        // LoadEntities();
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
            instantiatedInteractable = Interactable.Create(savedData);
        // bool ignoreRandomisation = instantiatedInteractable is IIgnoreTileRandomisation;
        bool ignoreRandomisation = instantiatedInteractable.Data.ignoreRandomisation;

        var transform = instantiatedInteractable.transform;
        transform.position = ignoreRandomisation ?
            new Vector3(Position.x + 0.5f, Position.y + 0.5f, 0)
            : new Vector3(Position.x + interactableOffset.x, Position.y + interactableOffset.y, 0);
        /*transform.localScale = new Vector3(
            !ignoreRandomisation && mirrored ? -1 : 1, 1, 1);*/

        instantiatedInteractable.OnTileLoad(this);
        savedData = instantiatedInteractable.SaveData;
    }

    public void LoadEntities()
    {
        entities.ForEach(entity =>
        {
            entity.gameObject.SetActive(true);
            entity.Load();
        });
        entities.Clear();
    }

    public void ClearInteractable()
    {
        if(instantiatedInteractable is not null) DestroyInstantiated();
        savedData = null;
    }
    
    // Убирает объект interactable этого тайла из мира
    public void UnloadInteractable()
    {
        savedData = instantiatedInteractable.SaveData.DeepClone();
        Object.DestroyImmediate(instantiatedInteractable.gameObject);
        instantiatedInteractable = null;
    }

    public void DestroyInstantiated()
    {
        instantiatedInteractable.Destroy();
    }


    public void HideInteractable(bool isHidden)
    {
        if(instantiatedInteractable is not null) instantiatedInteractable.SetActive(!isHidden);
    }

}
