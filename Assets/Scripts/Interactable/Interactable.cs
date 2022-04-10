using UnityEngine;

public class Interactable : MonoBehaviour
{
    // Public fields
    public InteractableData Data => data;

    public InteractableSaveData SaveData => saveData;
    
    // Private fields
    
    // Содержит сохраняемые поля объекта
    [SerializeReference, Header("Instance data")]
    protected InteractableSaveData saveData;
    // Содержит общие поля объекта
    [SerializeReference, Header("Data")]
    protected InteractableData data;

    protected Fader Fader;

    // Содержит ссылку на тайл в котором находится
    [SerializeReference]
    protected WorldTile tile;


    
    
    private void OnMouseEnter()
    {
        FadeIn();
    }
    
    private void OnMouseExit()
    {
        FadeOut();
    }


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
        saveData = InteractableSaveData.FromID(origin.id);
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


    #region Utils

    public virtual void SetActive(bool isHidden) => gameObject.SetActive(isHidden);

    public virtual void Destroy()
    {
        tile.savedData = null;
        tile.instantiatedInteractable = null;
        if(Application.isPlaying) GameObject.Destroy(gameObject);
        else DestroyImmediate(gameObject);
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