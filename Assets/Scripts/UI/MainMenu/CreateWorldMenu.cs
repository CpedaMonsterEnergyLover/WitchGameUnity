using System;
using System.Collections;
using System.Threading.Tasks;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WorldScenes;

public class CreateWorldMenu : MonoBehaviour
{
    public InputField seedInput;
    public SliderController seasonSliderController;
    public DifficultyToggleGroup difficultyToggleGroup;
    public WorldSizeToggleGroup worldSizeToggleGroup;
    public CustomButtonLightSticker difficultyLightSticker;
    public CustomButtonLightSticker worldSizeLightSticker;
    public Button continueButton;
    public Text confirmRewriteText;

    [Header("Loading bar")] 
    public BaseWorldScene sceneToLoad;
    public SceneLoadingBar loadingBar;

    public enum GameDifficulty
    {
        Easy,
        Normal,
        Hard
    }
    public enum WorldSize
    {
        Small,
        Standart,
        Huge
    }
    private bool _needsConfirm;


    public void OnSeedChanged() => continueButton.interactable = seedInput.text != string.Empty;

    private void OnEnable()
    {
        confirmRewriteText.gameObject.SetActive(false);
        _needsConfirm = false;
        difficultyLightSticker.Stick();
        worldSizeLightSticker.Stick();
        difficultyToggleGroup.Select(1);
        worldSizeToggleGroup.Select(1);
        seasonSliderController.slider.value = 7;
        seedInput.text = string.Empty;
    }

    public void OnCreateWorldClicked()
    {
        if (GameDataManager.HasSavedOverWorld())
        {
            confirmRewriteText.gameObject.SetActive(true);
            if (_needsConfirm)
                LoadNewWorldScene();
            _needsConfirm = true;
        }
        else
        {
            LoadNewWorldScene();
        }

    }
    
    private void LoadNewWorldScene()
    {
        _needsConfirm = false;
        loadingBar.Activate(6);
        loadingBar.SetPhase("Очистка данных");
        GameDataManager.ClearAllData();
        var scene = SceneManager.LoadSceneAsync(sceneToLoad.sceneName, LoadSceneMode.Additive);
        scene.completed += AfterSceneLoading;
    }

    private WorldManager wm;
    private GameSystem gs;
    private bool _startFade;
    
    private void AfterSceneLoading(AsyncOperation ao)
    {
        wm = FindObjectOfType<WorldManager>(true);
        gs = FindObjectOfType<GameSystem>(true);
        gs.gameObject.SetActive(false);
        wm.gameObject.SetActive(false);
        Generator generator = FindObjectOfType<Generator>(true);
        
        
        
         generator.Generate(
            new SelectedGeneratorSettings(
                difficultyToggleGroup.value, 
                worldSizeToggleGroup.value, 
                seedInput.text, 
                seasonSliderController.Value),
            loadingBar).GetAwaiter().OnCompleted(() =>
         {
            ScreenFader.SetContinuation(() =>
            {
                gs.gameObject.SetActive(true);
                wm.gameObject.SetActive(true);
                SceneManager.UnloadSceneAsync(2);
            });
            ScreenFader.StartFade();
         });
    }


}