using UnityEngine;

[System.Serializable]
public class InteractableData : ScriptableObject
{
    [Header("Interactable data")]
    public InteractableType interactableType;
    // Временный префаб для общего тестирования
    public GameObject prefab;
    [SerializeField]
    public new string name;
}
