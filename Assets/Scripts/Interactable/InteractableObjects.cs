using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObjects : MonoBehaviour
{
    #region Vars
    
    // Public fields
    public static InteractableData[][] Collection;
    
    // Private fields
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

    #endregion



    #region UnityMethods

    private void Awake()
    {
        if(inspectText is not null) InspectText = inspectText;
    }

    #endregion

    

    #region ClassMethods

    public void InitCollection()
    {
        Collection = new InteractableData[TypesMAX][];
        Collection[(int) InteractableType.Herb] = herbs;
        Collection[(int) InteractableType.Tree] = trees;
    }
    
    public static void InspectTextEnabled(bool enabled)
    {
        InspectText.enabled = enabled;
    }

    public static void SetInspectText(string text)
    {
        InspectText.text = text;
    }

    #endregion
}
