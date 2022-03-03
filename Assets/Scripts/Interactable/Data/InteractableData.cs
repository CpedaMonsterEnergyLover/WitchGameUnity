using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Interactable/Base")]
public class InteractableData : ScriptableObject
{
    [Header("Interactable data")] 
    public InteractableIdentifier identifier;
    // Временный префаб для общего тестирования
    public GameObject prefab;
    [SerializeField]
    public new string name;

    public bool ignoreOffset;
}
