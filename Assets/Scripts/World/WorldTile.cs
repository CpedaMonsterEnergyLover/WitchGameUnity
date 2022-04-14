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
    [SerializeField] private bool[] layers;
    [SerializeField] private Vector2Int position;
    [SerializeReference] public InteractableSaveData savedData;
    [SerializeReference] private List<EntitySaveData> savedEntities = new();

    [NonSerialized] private List<Entity> _cachedEntities = new();

    private List<Entity> CachedEntities { get; set; } = new();
    private Interactable InstantiatedInteractable { get; set; }

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
    public void OnPopped()
    {
        if(InstantiatedInteractable is null) return;
        savedData = InstantiatedInteractable.SaveData.DeepClone();
        Object.DestroyImmediate(InstantiatedInteractable.gameObject);
        InstantiatedInteractable = null;
    }

    #endregion

    public void InitAfterLoading()
    {
        _cachedEntities = new List<Entity>();
    }
    
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
        savedEntities = from.savedEntities;
        if(!keepChanges) WasChanged = false;
    }
    
    public void Load()
    {
        IsLoaded = true;
        LoadInteractable();
        WasChanged = true;
        LoadEntities();
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
        if(_cachedEntities.Count > 0)
            foreach (Entity entity in _cachedEntities) 
                entity.Load();
        _cachedEntities.Clear();
        
        foreach (EntitySaveData entitySaveData in savedEntities) 
            Entity.Create(entitySaveData);
        savedEntities.Clear();
    }

    // Runtime only
    public void SetInteractable(InteractableSaveData interactableSaveData)
    {
        WasChanged = true;
        DestroyInstantiated();
        savedData = interactableSaveData;
        Debug.Log($"SaveData: {savedData}, has interactable: {HasInteractable}" );
        if(IsLoaded) LoadInteractable();
    }

    public void CacheEntity([NotNull] Entity entity)
    {
        if (_cachedEntities.Contains(entity)) return;
        _cachedEntities.Add(entity);
    }

    public void RemoveEntityFromCache([NotNull] Entity entity)
    {
        if (!_cachedEntities.Contains(entity)) return;
        _cachedEntities.Remove(entity);
    }
    
    public void AddEntitySaveData([NotNull] EntitySaveData entitySaveData)
    {
        if (savedEntities.Contains(entitySaveData)) return;
        savedEntities.Add(entitySaveData);
        WasChanged = true;
    }

    public void RemoveEntitySaveData([NotNull] EntitySaveData entitySaveData)
    {
        if (!savedEntities.Contains(entitySaveData)) return;
        savedEntities.Remove(entitySaveData);
        WasChanged = true;
    }
    
    public void DestroyInstantiated()
    {
        if(InstantiatedInteractable is null) return;
        if (Application.isPlaying) Object.Destroy(InstantiatedInteractable.gameObject);
        else Object.DestroyImmediate(InstantiatedInteractable.gameObject);
        InstantiatedInteractable = null;
    }

    
}
