using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour
{
    #region Singleton

    public static CursorManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    
    #endregion

    public CursorMode Mode { set; get; }
    public static bool IsOverUI => EventSystem.current.IsPointerOverGameObject();


    private void Start()
    {
        ResetMode();
        CursorHoverCheck.ONCursorEnterUI += OnCursorEnterUI;
        CursorHoverCheck.ONCursorLeaveUI += OnCursorLeaveUI;
    }

    public void ResetMode()
    {
        Mode = IsOverUI ? CursorMode.HoverUI : CursorMode.InWorld;
    }
    
    private void OnCursorEnterUI()
    {
        Mode = CursorMode.HoverUI;
    }
    
    private void OnCursorLeaveUI()
    {
        Mode = CursorMode.InWorld;
    }
    
}

public enum CursorMode
{
    InWorld,
    HoverUI
}