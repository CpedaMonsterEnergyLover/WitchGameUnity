using System;
using UnityEngine;

[Serializable]
public class Interactable : MonoBehaviour
{
    #region Vars

    // Public fields
    public InteractableData Data => data;

    public InteractableSaveData SaveData => saveData;
    
    // Private fields
    
    // Содержит сохраняемые поля объекта
    [SerializeReference, Header("Instance data")]
    protected InteractableSaveData saveData;
    // Содержит общие поля объекта
    [SerializeField, Header("Data")]
    protected InteractableData data;

    protected Fader Fader;

    // Содержит ссылку на тайл в котором находится
    [SerializeReference]
    protected WorldTile tile;
    
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


    public static Interactable Create(InteractableSaveData saveData)
    {
        GameObject prefab = GameCollection.Interactables.Get(saveData.id);
        prefab = Instantiate(prefab, WorldManager.Instance.interactableTransform);
        Interactable interactable = prefab.GetComponent<Interactable>();

        if (saveData.initialized)
        {
            interactable.saveData = saveData;
        }
        else interactable.InitSaveData(interactable.Data);
        
        return interactable;
    }

    
    protected virtual void InitSaveData(InteractableData origin)
    {
        saveData = new InteractableSaveData(origin)
        {
            initialized = true
        };
    }
    
    public virtual void Interact(float value = 1.0f)
    {
        Debug.Log($"Interacting with {name}");
    }

    // Должен быть переопределен
    public virtual void OnTileLoad(WorldTile loadedTile)
    {
        Fader = GetComponentInChildren<Fader>();
        tile = loadedTile;
    }

    #endregion



    #region Utils

    public virtual void SetActive(bool isHidden) => gameObject.SetActive(isHidden);

    public virtual void Kill()
    {
        Debug.Log($"Kill interactable at {tile.Position}");
        tile.SetInteractable(null);
    }
    
    protected void FadeIn()
    {
        if (Fader is not null && Fader.IsFaded) Fader.FadeIn();
    }

    protected void FadeOut()
    {
        if (Fader is not null && Fader.IsFaded) Fader.FadeOut(0.15f);
    }

    #endregion
}