using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObjects : MonoBehaviour
{
    #region Vars
    
    // Public fields
    public static InteractableData Get(string id) =>  Collection[id];
    public static InteractableData Get(InteractableIdentifier identifier) =>  Collection[identifier.id];

    // Private fields

    // TODO: перенести в класс UI
    [Header("Текст всплывающей подсказки (ссылка)")]
    [SerializeField] 
    private Text inspectText;

    [Header("Игровые объекты")]
    [SerializeField]
    private List<InteractableData> herbs;
    [SerializeField]
    private List<InteractableData> trees;

    private const int TypesMAX = 2;
    private static Text InspectText;

    public static Dictionary<string, InteractableData> Collection;


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
        Collection = new Dictionary<string, InteractableData>();
        herbs.ForEach(i => Collection[i.identifier.id] = i);
        trees.ForEach(i => Collection[i.identifier.id] = i);
        Collection[""] = null;
    }

    public static void SetInspectTextEnabled(bool enabled) => InspectText.enabled = enabled;
    
    public static void SetInspectText(string text = "")
    {
        InspectText.text = text;
    }

    #endregion
}
