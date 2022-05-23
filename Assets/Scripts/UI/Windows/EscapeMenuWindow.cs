using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenuWindow : BaseWindow
{
    public GameObject panelToDisable;
    
    public List<Component> toDisable = new();
        
    private TemporaryDismissData _dismissData;
    
    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        WorldScene.LoadMainMenu();
    }

    public void SaveGame()
    {
        async UniTask Run()
        {
            panelToDisable.SetActive(false);
            await ScreenFader.Instance.StartFade();
            await GameDataManager.SaveAll();
            _dismissData = _dismissData?.ShowAll();
            Toggle();
            panelToDisable.SetActive(true);
            await ScreenFader.Instance.StopFade();
        }
        Run().Forget();
    }


    public override void Toggle()
    {
        base.Toggle();
        bool isPaused = IsActive;

        _dismissData = isPaused ? 
            new TemporaryDismissData().Add(toDisable).HideAll() : 
            _dismissData?.ShowAll();
        
        Time.timeScale = isPaused ? 0 : 1;
    }
}
