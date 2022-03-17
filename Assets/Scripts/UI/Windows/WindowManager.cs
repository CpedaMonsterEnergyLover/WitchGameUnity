using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField]
    private List<BaseWindow> windows;
    public static List<BaseWindow> All { get; private set; } 
    
    private static readonly BaseWindow[] Windows = new BaseWindow[16];

    
    
    private void Awake()
    {
        All = windows;
        windows.ForEach(window =>
        {
            Windows[window.Identifier] = window;
        });        
    }

    
    
    public static bool IsActive(WindowIdentifier id) => Windows[(int)id].IsActive;

    public static T Get<T>(WindowIdentifier id) where T : BaseWindow
        => (T)Windows[(int) id];
    
    public static BaseWindow Get(WindowIdentifier id)
        => Windows[(int) id];

    public static void SetActive(WindowIdentifier id, bool isActive) 
        => Windows[(int)id].SetActive(isActive);

    public static void Toggle(WindowIdentifier id) 
        => Windows[(int)id].Toggle();
}
