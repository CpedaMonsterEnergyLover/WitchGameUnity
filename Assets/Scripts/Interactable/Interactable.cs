using System;
using System.Text;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Interactable : MonoBehaviour
{
    #region Vars

    // Public fields
    public InteractableData Data => data;
    public InteractableSaveData InstanceData => instanceData;
    
    // Private fields
    
    [SerializeReference]
    // Содержит сохраняемые поля объекта
    protected InteractableSaveData instanceData;
    [SerializeReference]
    // Содержит общие поля объекта
    protected InteractableData data;
    
    protected bool InitComplete;
    protected Fader _fader;
    
    // Содержит ссылку на тайл в котором находится
    protected WorldTile tile;


    #endregion



    #region UnityMethods

    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
            Interact();
        else 
            InteractableObjects.SetInspectTextEnabled(true);
    }
    
    private void OnMouseEnter()
    {
        Inspect();
    }
    
    private void OnMouseExit()
    {
        StopInspect();
    }

    #endregion



    #region ClassMethods
    
    public static Interactable Create(Transform parent, InteractableSaveData saveData)
    {
        GameObject prefab = InteractableObjects.Get(saveData.identifier).prefab;
        GameObject instantiatedObject = Instantiate(prefab, parent);
        instantiatedObject.transform.rotation = Quaternion.identity;
        Interactable addedScript = saveData.identifier.type switch
        {
            InteractableType.Herb => instantiatedObject.AddComponent<WoodTree>(),
            InteractableType.Tree => instantiatedObject.AddComponent<WoodTree>(),
            InteractableType.Rock => instantiatedObject.AddComponent<WoodTree>(),
            _ => throw new ArgumentOutOfRangeException("Unknown interactable type", new Exception())
        };
        addedScript.LoadData(saveData);
        return addedScript;
    } 

    private InteractableSaveData LoadData(InteractableSaveData saveData)
    {
        // Загружает данные, зависящие от типа объекта
        data = InteractableObjects.Get(saveData.identifier);
        instanceData = saveData;

        // Если объект был создан пустой, то есть в data отсутствует instanceID
        // Инициализирует начальные значения
        if (IsNew()) InitInstanceData(saveData);

        InitComplete = true;

        return InstanceData;
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
        _fader = GetComponentInChildren<Fader>();
        tile = loadedTile;
    }
    
    #endregion



    #region Utils

    public void SetActive(bool isHidden) => gameObject.SetActive(isHidden);

    protected void Destroy()
    {
        tile.savedData = null;
        tile.instantiatedInteractable = null;
        GameObject.Destroy(gameObject);
    }
    
    private void FadeIn()
    {
        if (_fader is not null && _fader.IsFaded) _fader.FadeIn();
    }

    private void FadeOut()
    {
        if (_fader is not null && _fader.IsFaded) _fader.FadeOut();
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