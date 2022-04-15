﻿using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenuWindow : BaseWindow
{
    public GameObject gameSystem;
    public SceneLoadingBar loadingBar;
    
    
    public List<Component> toDisable = new();
        
    private TemporaryDismissData _dismissData;
    
    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(2);
    }

    public void SaveGame()
    {
        loadingBar.Activate(3);
        
        ScreenFader.Instance.FadeUnscaled(true).GetAwaiter().OnCompleted(async () =>
        {
            loadingBar.SetPhase("Сохранение");
            await GameDataManager.SaveAll(loadingBar);
            Time.timeScale = 1;
            _dismissData = _dismissData?.ShowAll();
            loadingBar.gameObject.SetActive(false);
            await ScreenFader.Instance.FadeUnscaled(false);
        });
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
