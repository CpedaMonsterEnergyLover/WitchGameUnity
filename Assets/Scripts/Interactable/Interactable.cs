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

    #endregion



    #region UnityMethods

    

    #endregion



    #region ClassMethods

    private void Init(InteractableSaveData data)
    {
        // Загружает данные, зависящие от типа объекта
        Data = InteractableObjects.Collection[(int) data.interactableType][data.interactableID];
        InstanceData = data;

        // Если объект был создан пустой, то есть в data отсутствует instanceID
        // Инициализирует начальные значения
        if (IsNew()) InitInstanceData(data);
        
        //При загрузке тайла, пересчитывает данные объекта
        OnTileLoad();

        InitComplete = true;
    }
    
    protected virtual void Interact()
    {
        //Debug.Log("Interacting with " + GetData.name);
    }

    #endregion



    #region Utils
    
    private void LoadData(InteractableType type, int id) { }
    
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
