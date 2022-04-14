using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenuWindow : BaseWindow
{
    public GameObject gameSystem;
    
    public List<Component> toDisable = new();
        
    private TemporaryDismissData _dismissData;
    
    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadMenuRoutine());
    }

    private IEnumerator LoadMenuRoutine()
    {
        var ao = SceneManager.LoadSceneAsync(2);
        yield  return ao;
        StartCoroutine(UnloadSceneRoutine());
    }
    
    private IEnumerator UnloadSceneRoutine()
    {
        var ao = SceneManager.UnloadSceneAsync(WorldManager.Instance.worldScene.sceneName);
        yield return ao;
    }

    public void SaveGame()
    {
        GameDataManager.SaveAll();
        Toggle();
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
