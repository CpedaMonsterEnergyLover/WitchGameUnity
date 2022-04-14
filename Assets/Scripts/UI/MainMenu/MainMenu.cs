using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WorldScenes;

public class MainMenu : MonoBehaviour
{
    public Button loadGameButton;
    public BaseWorldScene sceneToLoad;
    
    private void OnEnable()
    {
        loadGameButton.interactable = GameDataManager.HasSavedOverWorld();
    }

    public void LoadSavedGame()
    {
        SceneManager.LoadScene(sceneToLoad.sceneName);
    }

    public void ExitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}