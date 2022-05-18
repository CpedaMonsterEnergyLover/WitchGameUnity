using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WorldScenes;

public class MainMenu : MonoBehaviour
{
    public Button loadGameButton;
    public WorldScene sceneToLoad;
    
    private void OnEnable()
    {
        loadGameButton.interactable = GameDataManager.HasSavedOverWorld();
    }

    public void LoadSavedGame()
    {
        async UniTask Load()
        {
            await ScreenFader.Instance.StartFade();
            SceneManager.LoadScene(sceneToLoad.sceneName);
        }   
        Load().Forget();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}