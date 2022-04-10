using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Interactable/Base")]
public class InteractableData : ScriptableObject
{
    [Header("Interactable data")] 
    public string id;
    public new string name;
    [Range(0, 10)]
    public float interactingTime;

    public bool ignoreRandomisation;



    protected virtual T Test<T>() where T : InteractableSaveData
    {
        return CreateInstance<T>();
    }
}
