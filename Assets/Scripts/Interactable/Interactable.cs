using System;
using System.Text;
using UnityEngine;
using Object = System.Object;
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
    protected bool loaded;

    #endregion



    #region UnityMethods

    private void OnMouseDown()
    {
        Interact();
    }

    /*private void Start()
    {
        Init();
    }*/

    #endregion



    #region ClassMethods
    
    public static GameObject Create(GameObject prefab, Transform parent, InteractableSaveData saveData)
    {
        GameObject instantiatedObject = Instantiate(prefab, parent);
        instantiatedObject.transform.position = saveData.position;
        instantiatedObject.transform.rotation = Quaternion.identity;
        Interactable addedScript = saveData.interactableType switch
        {
            InteractableType.Herb => instantiatedObject.AddComponent<WoodTree>(),
            InteractableType.Tree => instantiatedObject.AddComponent<WoodTree>(),
            InteractableType.Rock => instantiatedObject.AddComponent<WoodTree>(),
            _ => throw new ArgumentOutOfRangeException("Unknown interactable type", new Exception())
        };
        addedScript.Init(saveData);
        return instantiatedObject;
    } 

    private void Init(InteractableSaveData data)
    {
        // Загружает данные, зависящие от типа объекта
        Data = InteractableObjects.Collection[(int) data.interactableType][data.interactableID];
        InstanceData = data;

        // Если объект был создан пустой, то есть в data отсутствует instanceID
        // Инициализирует начальные значения
        if (IsNew()) InitInstanceData(data);
        
        // При загрузке тайла, производит все свойственные ему действия
        // По прошествии времени
        OnTileLoad();

        InitComplete = true;
    }
    
    protected virtual void Interact()
    {
        //Debug.Log("Interacting with " + GetData.name);
    }

    #endregion



    #region Utils

    // Должен быть переопределен 
    protected virtual void InitInstanceData(InteractableSaveData data) { }
    
    // Должен быть переопределен
    protected virtual void OnTileLoad() { }

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
