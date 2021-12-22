using System;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

public class Interactable : MonoBehaviour
{
    #region Vars

    // Public fields
    public InteractableData Data => data;
    public InteractableSaveData InstanceData => instanceData;
    
    // Private fields
    
    // Содержит сохраняемые поля объекта
    [SerializeReference, Header("Instance data")]
    protected InteractableSaveData instanceData;
    // Содержит общие поля объекта
    [SerializeReference, Header("Data")]
    protected InteractableData data;

    protected Fader Fader;

    // Содержит ссылку на тайл в котором находится
    [SerializeReference]
    protected WorldTile Tile;
    
    #endregion



    #region UnityMethods
    
    private void OnMouseEnter()
    {
        FadeIn();
    }
    
    private void OnMouseExit()
    {
        FadeOut();
    }

    #endregion



    #region ClassMethods

    public static GameObject GetPrefab(InteractableIdentifier identifier)
    {
        return GameObjectsCollection.GetInteractable(identifier).prefab;
    }
    
    public static Interactable Create(InteractableSaveData saveData)
    {
        GameObject prefab = GameObjectsCollection.GetInteractable(saveData.identifier).prefab;
        GameObject instantiatedObject = Instantiate(prefab, WorldManager.Instance.GameObjectsTransform);
        instantiatedObject.transform.rotation = Quaternion.identity;
        Interactable addedScript = saveData.identifier.type switch
        {
            InteractableType.Herb => instantiatedObject.AddComponent<Herb>(),
            InteractableType.Tree => instantiatedObject.AddComponent<WoodTree>(),
            InteractableType.Rock => instantiatedObject.AddComponent<WoodTree>(),
            InteractableType.CropBed => instantiatedObject.AddComponent<CropBed>(),
            _ => throw new ArgumentOutOfRangeException("Unknown interactable type", new Exception())
        };
        addedScript.LoadData(saveData);
        return addedScript;
    } 

    private void LoadData(InteractableSaveData saveData)
    {
        // Загружает данные, зависящие от типа объекта
        data = GameObjectsCollection.GetInteractable(saveData.identifier);
        instanceData = saveData;

        // Если объект был создан пустой, то есть в data отсутствует instanceID
        // Инициализирует начальные значения
        if (IsNew()) InitInstanceData(saveData);
    }

    public virtual void Interact(float value = 1.0f)
    {
    }

    public virtual bool AllowInteract() => true;


    // Должен быть переопределен
    public virtual void OnTileLoad(WorldTile loadedTile)
    {
        Fader = GetComponentInChildren<Fader>();
        Tile = loadedTile;
    }
    
    #endregion



    #region Utils

    public virtual void SetActive(bool isHidden) => gameObject.SetActive(isHidden);

    public virtual void Destroy()
    {
        Tile.savedData = null;
        Tile.instantiatedInteractable = null;
        GameObject.Destroy(gameObject);
    }
    
    protected void FadeIn()
    {
        if (Fader is not null && Fader.IsFaded) Fader.FadeIn();
    }

    protected void FadeOut()
    {
        if (Fader is not null && Fader.IsFaded) Fader.FadeOut(0.15f);
    }
    
    // Должен быть переопределен 
    protected virtual void InitInstanceData(InteractableSaveData saveData)
    {
        instanceData.instanceID = GenerateID();
    }

    // Проверяет, был ли инициализирован объект
    private bool IsNew()
    {
        return string.IsNullOrEmpty(InstanceData.instanceID);
    }
    
    // Генерирует уникальный айди
    protected string GenerateID()
    {
        return new StringBuilder()
            .Append(DateTime.Now.Date.ToString("MM:dd:yyyy:"))
            .Append(DateTime.Now.ToLongTimeString())
            .Append(Random.Range(0, int.MaxValue))
            .ToString();
    }
    
    #endregion
}



[Serializable]
public class InteractableIdentifier
{
    public InteractableType type;
    public string id;

    public InteractableIdentifier(InteractableType type, string id)
    {
        this.type = type;
        this.id = id;
    }

    public override string ToString()
    {
        return $"{type}:{id}";
    }
}