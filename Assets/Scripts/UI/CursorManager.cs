using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour
{
    #region Singleton

    public static CursorManager Instance;


    private void Awake()
    {
        if (Instance is null) Instance = this;
        else Debug.LogError("Found multiple instances of CursorManager");
        CursorHoverCheck.ONCursorEnterUI += OnCursorEnterUI;
        CursorHoverCheck.ONCursorLeaveUI += OnCursorLeaveUI;
    }
    
    #endregion

    public CursorMode Mode;
    public bool InteractAllowed => InMode(CursorMode.InWorld);
    public bool InMode(CursorMode mode) => Mode == mode;
    public bool IsOverUI => EventSystem.current.IsPointerOverGameObject();


    private void Start()
    {
        ResetMode();
    }

    public void ResetMode()
    {
        Mode = IsOverUI ? CursorMode.HoverUI : ItemPicker.Instance.itemSlot.HasItem ? CursorMode.HoldItem : CursorMode.InWorld;
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
    HoldItem,
    InWorld,
    HoverUI
}