using UnityEngine;

[System.Serializable]
public class InteractableData : ScriptableObject
{
    [Header("Interactable data")]
    public InteractableType interactableType ;
    [SerializeField]
    public new string name;
}
