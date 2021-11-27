using UnityEngine;
using UnityEngine.UI;

public class InteractableObjects : MonoBehaviour
{
    #region Vars
    
    // Public fields
    public static InteractableData Get(InteractableType type, int id) => Collection[(int) type][id];
    public static InteractableData Get(InteractableIdentifier identifier) => Collection[(int) identifier.type][identifier.id];

    // Private fields

    // TODO: перенести в класс UI
    [Header("Текст всплывающей подсказки (ссылка)")]
    [SerializeField] 
    private Text inspectText;

    [Header("Игровые объекты")]
    [SerializeField]
    private InteractableData[] herbs;
    [SerializeField]
    private InteractableData[] trees;

    private const int TypesMAX = 2;
    private static Text InspectText;

    private static InteractableData[][] Collection = new InteractableData[TypesMAX][];

    
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
        Collection[(int) InteractableType.Herb] = herbs;
        Collection[(int) InteractableType.Tree] = trees;
    }

    public static void SetInspectTextEnabled(bool enabled) => InspectText.enabled = enabled;
    
    public static void SetInspectText(string text = "")
    {
        InspectText.text = text;
    }

    #endregion
}
