using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameObjectsCollection : MonoBehaviour
{
    #region Vars
    
    // Public fields
    public static InteractableData GetInteractable(string id) =>  InteractableCollection[id];
    public static InteractableData GetInteractable(InteractableIdentifier identifier) =>  InteractableCollection[identifier.id];
    
    public static ItemData GetItem(string id) =>  ItemCollection[id];
    public static ItemData GetItem(ItemIdentifier identifier) =>  ItemCollection[identifier.id];

    // Private fields

    // TODO: перенести в класс UI
    [Header("Текст всплывающей подсказки (ссылка)")]
    [SerializeField] 
    private Text inspectText;

    [Header("Игровые объекты")]
    [Header("Травы")]
    [SerializeField]
    private List<InteractableData> herbs;
    [Header("Деревья")]
    [SerializeField]
    private List<InteractableData> trees;
    [Header("Предметы")]
    [SerializeField]
    private List<ItemData> items;

    private static Text InspectText;

    public static Dictionary<string, InteractableData> InteractableCollection;
    public static Dictionary<string, ItemData> ItemCollection;


    #endregion



    #region UnityMethods

    private void Awake()
    {
        if (inspectText is not null)
        {
            InspectText = inspectText;
            inspectText.enabled = false;
        }
    }

    #endregion

    

    #region ClassMethods

    public void InitCollection()
    {
        // Interactable collection
        InteractableCollection = new Dictionary<string, InteractableData>();
        herbs.ForEach(i => InteractableCollection[i.identifier.id] = i);
        trees.ForEach(i => InteractableCollection[i.identifier.id] = i);
        InteractableCollection[""] = null;
        
        // Items collection
        ItemCollection = new Dictionary<string, ItemData>();
        items.ForEach(i => ItemCollection[i.identifier.id] = i);
    }

    public static void SetInspectTextEnabled(bool enabled) => InspectText.enabled = enabled;
    
    public static void SetInspectText(string text = "")
    {
        InspectText.text = text;
    }

    #endregion
}
