using System;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Interactable : MonoBehaviour
{
    #region Vars

    // Public fields
    public InteractableData Data => data;
    public InteractableSaveData InstanceData => instanceData;
    
    // Private fields
    
    // Содержит сохраняемые поля объекта
    [SerializeReference]
    protected InteractableSaveData instanceData;
    // Содержит общие поля объекта
    [SerializeReference]
    protected InteractableData data;

    protected Fader Fader;

    // Содержит ссылку на тайл в котором находится
    protected WorldTile Tile;


    #endregion



    #region UnityMethods
    
    private void OnMouseOver()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (Input.GetMouseButton(0)/* || Input.GetMouseButtonDown(0)*/)
            Interact();
        else 
            InteractableObjects.SetInspectTextEnabled(true);
    }
    
    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        //Inspect();
    }
    
    private void OnMouseExit()
    {
        StopInspect();
    }

    #endregion



    #region ClassMethods
    
    public static Interactable Create(InteractableSaveData saveData)
    {
        GameObject prefab = InteractableObjects.Get(saveData.identifier).prefab;
        GameObject instantiatedObject = Instantiate(prefab, WorldManager.GameObjectsTransform);
        instantiatedObject.transform.rotation = Quaternion.identity;
        Interactable addedScript = saveData.identifier.type switch
        {
            InteractableType.Herb => instantiatedObject.AddComponent<Herb>(),
            InteractableType.Tree => instantiatedObject.AddComponent<WoodTree>(),
            InteractableType.Rock => instantiatedObject.AddComponent<WoodTree>(),
            _ => throw new ArgumentOutOfRangeException("Unknown interactable type", new Exception())
        };
        addedScript.LoadData(saveData);
        return addedScript;
    } 

    private void LoadData(InteractableSaveData saveData)
    {
        // Загружает данные, зависящие от типа объекта
        data = InteractableObjects.Get(saveData.identifier);
        instanceData = saveData;

        // Если объект был создан пустой, то есть в data отсутствует instanceID
        // Инициализирует начальные значения
        if (IsNew()) InitInstanceData(saveData);
    }

    protected virtual void Interact()
    {
        InteractableObjects.SetInspectTextEnabled(false);
    }
    
    protected virtual void Inspect()
    {
        FadeIn();
        InteractableObjects.SetInspectText(Data.name);
        InteractableObjects.SetInspectTextEnabled(true);
    }

    protected virtual void StopInspect()
    {
        FadeOut();
        InteractableObjects.SetInspectTextEnabled(false);
    }

    // Должен быть переопределен
    public virtual void OnTileLoad(WorldTile loadedTile)
    {
        Fader = GetComponentInChildren<Fader>();
        Tile = loadedTile;
    }
    
    #endregion



    #region Utils

    public virtual void SetActive(bool isHidden) => gameObject.SetActive(isHidden);

    protected virtual void Destroy()
    {
        Tile.savedData = null;
        Tile.instantiatedInteractable = null;
        GameObject.Destroy(gameObject);
    }
    
    private void FadeIn()
    {
        if (Fader is not null && Fader.IsFaded) Fader.FadeIn();
    }

    private void FadeOut()
    {
        if (Fader is not null && Fader.IsFaded) Fader.FadeOut(0.15f);
    }
    
    // Должен быть переопределен 
    protected virtual void InitInstanceData(InteractableSaveData saveData) { }

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