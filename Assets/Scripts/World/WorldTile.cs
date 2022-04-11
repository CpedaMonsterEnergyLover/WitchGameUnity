using System.Collections.Generic;
using System;
using JetBrains.Annotations;
using TileLoading;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


[Serializable]
public class WorldTile : ICacheable
{
    [SerializeField] public Vector2 interactableOffset;
    [SerializeReference] public InteractableSaveData savedData;
    [SerializeField] private bool[] layers;
    [SerializeField] private Vector2Int position;

    [NonSerialized] public List<EntitySaveData> savedEntities = new();

    // TODO: manage this 
    public Interactable InstantiatedInteractable { get; private set; }
    public List<Entity> CachedEntities { get; private set; } = new();

    public bool[] Layers => layers;
    public Vector2Int Position
    {
        get => position;
        set => position = value;
    }
    public bool HasInteractable => savedData is not null;
    
    public bool WasChanged { get; private set; }


    #region ICacheable

    public bool IsLoaded { get; set; }
    public bool IsCached { get; set; }
    public GameObject GetCacheableItem => InstantiatedInteractable.gameObject;
    public void LeaveCache()
    {
        savedData = InstantiatedInteractable.SaveData.DeepClone();
        Object.DestroyImmediate(InstantiatedInteractable.gameObject);
        InstantiatedInteractable = null;
    }

    #endregion
    
    public WorldTile(int x, int y, bool[] tiles, InteractableData interactableData)
    {
        position = new Vector2Int(x, y);
        layers = tiles;
        savedData = interactableData is null ? null : 
            new InteractableSaveData(interactableData);
        interactableOffset =  new Vector2(Random.value * 0.6f + 0.2f, Random.value * 0.6f + 0.2f);
    }

    public void SetLayer(int layerIndex, bool value)
    {
        Layers[layerIndex] = value;
    }

    public void MergeData(WorldTile from, bool keepChanges)
    {
        interactableOffset = from.interactableOffset;
        savedData = from.savedData;
        layers = from.Layers;
        position = from.Position;
        if(!keepChanges) WasChanged = false;
    }
    
    public void Load()
    {
        IsLoaded = true;
        // LoadEntities();
        LoadInteractable();
        WasChanged = true;
    }

    public void LoadInteractable()
    {
        if (savedData is null) return;

        if (!GameCollection.Interactables.ContainsID(savedData.id))
        {
            Debug.LogWarning($"Попытка загрузки Interactable с несуществующим айди {savedData.id}");
            savedData = null;
            return;
        }

        if (IsCached)
            InstantiatedInteractable.SetActive(true);
        else
            InstantiatedInteractable = Interactable.Create(savedData);
        
        // bool ignoreRandomisation = instantiatedInteractable is IIgnoreTileRandomisation;
        bool ignoreRandomisation = InstantiatedInteractable.Data.ignoreRandomisation;
        var transform = InstantiatedInteractable.transform;
        transform.position = ignoreRandomisation ?
            new Vector3(Position.x + 0.5f, Position.y + 0.5f, 0)
            : new Vector3(Position.x + interactableOffset.x, Position.y + interactableOffset.y, 0);

        InstantiatedInteractable.OnTileLoad(this);
        savedData = InstantiatedInteractable.SaveData;
    }

    public void LoadEntities()
    {
        CachedEntities.ForEach(entity =>
        {
            entity.gameObject.SetActive(true);
            entity.Load();
        });
        CachedEntities.Clear();
    }

    // Runtime only
    public void SetInteractable(InteractableSaveData interactableSaveData)
    {
        DestroyInstantiated();
        savedData = interactableSaveData;
        WasChanged = true;
        if(IsLoaded) LoadInteractable();
    }

    // Runtime only
    public void AddEntity([NotNull] EntitySaveData entitySaveData)
    {
        
    }
    
    
    public void DestroyInstantiated()
    {
        if(InstantiatedInteractable is null) return;
        if(Application.isPlaying) Object.Destroy(InstantiatedInteractable);
        else Object.DestroyImmediate(InstantiatedInteractable);
    }

}
