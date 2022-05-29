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
    [SerializeField] private float generated;
    [SerializeField] public Vector2 interactableOffset;
    [SerializeField] private bool[] layers;
    [SerializeField] private Vector2Int position;
    [SerializeField] public long lastLoadedMinute;
    [SerializeReference] public InteractableSaveData savedData;
    [SerializeReference] private List<EntitySaveData> savedEntities = new();
    [SerializeField] private Color color;
    [SerializeField] private TileResourceData resourceData = new ();
    [SerializeReference] private AbstractBiome biome;


    public TileResourceData ResourceData => resourceData;
    public Vector2 Center => Position + new Vector2(0.5f, 0.5f);
    private List<Entity> CachedEntities { get; set; } = new ();
    private Interactable InstantiatedInteractable { get; set; }
    public bool IsBlockedForLoading { get; set; }
    public Color Color => color;
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
        Object.Destroy(InstantiatedInteractable.gameObject);
        InstantiatedInteractable = null;
    }

    #endregion

    public WorldTile(int x, int y, bool[] tiles, Color color, AbstractBiome biome)
    {
        position = new Vector2Int(x, y);
        layers = tiles;
        interactableOffset =  new Vector2(Random.value * 0.6f + 0.2f, Random.value * 0.6f + 0.2f);
        this.color = color;
        this.biome = biome;
        generated = biome is null ? -1 : Random.value;
    }

    public void SetLayer(int layerIndex, bool value)
    {
        Layers[layerIndex] = value;
    }

    public void MergeData(WorldTile from, bool keepChanged)
    {
        interactableOffset = from.interactableOffset;
        savedData = from.savedData;
        layers = from.Layers;
        position = from.Position;
        savedEntities = from.savedEntities;
        if(!keepChanged) WasChanged = false;
    }
    
    public void Load()
    {
        if (generated >= 0)
        {
            if (biome.GetInteractable(out InteractableSaveData saveData, generated))
                savedData = saveData;
            generated = -1;
        }

        IsLoaded = true;
        LoadInteractable();
        WasChanged = true;
        if(biome is not null && biome.hasItemDrops) SpawnResource();
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
        {
            InstantiatedInteractable = Interactable.Create(savedData);
            if(InstantiatedInteractable is IColorableInteractable inheritor) inheritor.SetColor(color);    
            bool ignoreRandomisation = InstantiatedInteractable.Data.ignoreTileRandomisation;
            InstantiatedInteractable.transform.position = ignoreRandomisation ?
                new Vector3(Position.x + 0.5f, Position.y + 0.5f, 0)
                : new Vector3(Position.x + interactableOffset.x, Position.y + interactableOffset.y, 0);
        }
        
        InstantiatedInteractable.OnTileLoad(this);
        savedData = InstantiatedInteractable.SaveData;
    }

    public void LoadEntities()
    {
        if (CachedEntities is null)
        {
            CachedEntities = new List<Entity>();
        }
        else if (CachedEntities.Count > 0)
        {
            foreach (Entity entity in CachedEntities) 
                entity.Load();
            CachedEntities.Clear();
        }
        
        foreach (EntitySaveData entitySaveData in savedEntities) 
            Entity.Create(entitySaveData);
        savedEntities.Clear();
    }

    // Runtime only
    public void SetInteractable(InteractableSaveData interactableSaveData)
    {
        generated = -1;
        WasChanged = true;
        DestroyInstantiated();
        savedData = interactableSaveData;
        if(IsLoaded) LoadInteractable();
    }

    public void CacheEntity([NotNull] Entity entity)
    {
        if (CachedEntities.Contains(entity)) return;
        CachedEntities.Add(entity);
    }

    public void RemoveEntityFromCache([NotNull] Entity entity)
    {
        if (!CachedEntities.Contains(entity)) return;
        CachedEntities.Remove(entity);
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
        if (Application.isPlaying) GameObject.Destroy(InstantiatedInteractable.gameObject);
        else GameObject.DestroyImmediate(InstantiatedInteractable.gameObject);
        InstantiatedInteractable = null;
    }

    private void SpawnResource()
    {
        if(!resourceData.SpawnResource || 
           resourceData.SpawnMinute > Timeline.TotalMinutes) return;
        resourceData.HasResource = true;
        resourceData.SpawnResource = false;
        savedEntities.Add(new ResourceItemEntitySaveData
        {
            id = "resource_item_entity",
            position = Center,
            preInitialised = true,
            spawnTile = this,
            item = Item.Create(biome.GetDrop().identifier),
            amount = 1,
        });
    }
    
}
