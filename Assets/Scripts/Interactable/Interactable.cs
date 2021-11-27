using System;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

public class Interactable : MonoBehaviour
{
    #region Vars

    // Public fields
    // Содержит общие поля объекта
    public InteractableData Data;
    public InteractableData GetData() => Data;
    
    [SerializeReference]
    // Содержит сохраняемые поля объекта
    public InteractableSaveData InstanceData;
    public InteractableSaveData GetInstanceData() => InstanceData;
    
    // Private fields
    protected bool InitComplete = false;
    protected Fader _fader;


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
        Data = InteractableObjects.Get(saveData.identifier);
        InstanceData = saveData;

        // Если объект был создан пустой, то есть в data отсутствует instanceID
        // Инициализирует начальные значения
        if (IsNew()) InitInstanceData(saveData);
        
        // При загрузке тайла, производит все свойственные ему действия
        // По прошествии времени
        OnTileLoad();

        InitComplete = true;

        return GetInstanceData();
    }

    protected virtual void Interact()
    {
        InteractableObjects.SetInspectTextEnabled(false);
    }
    
    protected virtual void Inspect()
    {
        FadeIn();
        InteractableObjects.SetInspectText(GetData().name);
        InteractableObjects.SetInspectTextEnabled(true);
    }

    protected virtual void StopInspect()
    {
        FadeOut();
        InteractableObjects.SetInspectTextEnabled(false);
    }

    // Должен быть переопределен
    public virtual void OnTileLoad()
    {
        _fader = GetComponentInChildren<Fader>();
    }
    
    #endregion



    #region Utils

    public void SetActive(bool isHidden) => gameObject.SetActive(isHidden);
    
    private void FadeIn()
    {
        if (_fader is not null && _fader.IsFaded) _fader.FadeIn();
    }

    private void FadeOut()
    {
        if (_fader is not null && _fader.IsFaded) _fader.FadeOut();
    }
    
    // Должен быть переопределен 
    protected virtual void InitInstanceData(InteractableSaveData data) { }

    // Проверяет, был ли инициализирован объект
    private bool IsNew()
    {
        return string.IsNullOrEmpty(GetInstanceData().instanceID);
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
    public int id;

    public InteractableIdentifier(InteractableType type, int id)
    {
        this.type = type;
        this.id = id;
    }

    public override string ToString()
    {
        return $"{type}:{id}";
    }
}