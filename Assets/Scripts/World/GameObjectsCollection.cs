/*
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
    [Header("Игровые объекты")]
    [Header("Травы")]
    [SerializeField]
    private List<InteractableData> herbs;
    [Header("Деревья")]
    [SerializeField]
    private List<InteractableData> trees;
    [Header("Другое")]
    [SerializeField]
    private List<InteractableData> others;
    [Header("Предметы")]
    [SerializeField]
    private List<ItemData> items;

    public static Dictionary<string, InteractableData> InteractableCollection;
    public static Dictionary<string, ItemData> ItemCollection;


    #endregion
    
    

    #region ClassMethods

    public void InitCollection()
    {
        // Interactable collection
        InteractableCollection = new Dictionary<string, InteractableData>();
        herbs.ForEach(i => InteractableCollection[i.identifier.id] = i);
        trees.ForEach(i => InteractableCollection[i.identifier.id] = i);
        others.ForEach(i => InteractableCollection[i.identifier.id] = i);
        InteractableCollection[""] = null;
        
        // Items collection
        ItemCollection = new Dictionary<string, ItemData>();
        items.ForEach(i => ItemCollection[i.identifier.id] = i);
    }
    
    #endregion
}
*/
