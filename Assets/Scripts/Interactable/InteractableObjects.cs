using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObjects : MonoBehaviour
{
    #region Vars
    
    // Public fields
    public static readonly InteractableData[][] Collection = new InteractableData[TypesMAX][];
    
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
    
    private static InteractableData[] _herbs;
    private static InteractableData[] _trees;

    #endregion



    #region UnityMethods

    private void Awake()
    {
        _herbs = herbs;
        _trees = trees;
        if(inspectText is not null) InspectText = inspectText;
    }

    #endregion

    

    #region ClassMethods

    public static void InitCollection()
    {
        Collection[(int) InteractableType.Herb] = _herbs;
        Collection[(int) InteractableType.Tree] = _trees;
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
