using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.EventSystems;
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
    public EventSystem eventSystem;

    [Header("Loading bar")] 
    public BaseWorldScene sceneToLoad;
    public LoadingBar loadingBar;
    public GameObject panelToDisable;
    public List<string> loadingPhases;

    [Header("Screen fader")] 
    public ScreenFader screenFader;


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
            {
                loadingBar.Activate(loadingPhases);
                LoadNewWorldScene().Forget();
            }
            _needsConfirm = true;
        }
        else
        {
            loadingBar.Activate(loadingPhases);
            LoadNewWorldScene().Forget();
        }

    }
    
    private async UniTask LoadNewWorldScene()
    {
        screenFader.Init();
        eventSystem.enabled = false;
        panelToDisable.SetActive(false);
        _needsConfirm = false;
        GameDataManager.ClearAllData();
        await SceneManager.LoadSceneAsync(sceneToLoad.sceneName, LoadSceneMode.Additive);
        WorldManager wm = WorldManager.Instance;
        GameSystem gs = GameSystem.Instance;
        Generator generator = Generator.Instance;
        gs.gameObject.SetActive(false);
        wm.gameObject.SetActive(false);
        
        await generator.Generate(
            new SelectedGeneratorSettings(
                difficultyToggleGroup.value,
                worldSizeToggleGroup.value,
                seedInput.text,
                seasonSliderController.Value),
            loadingBar);
        
        ScreenFader.Instance.SetContinuation(() =>
        {
            loadingBar.Stop();
            SceneManager.UnloadSceneAsync(2);
            gs.gameObject.SetActive(true);
            wm.gameObject.SetActive(true);
        });

        await UniTask.SwitchToMainThread();
        await UniTask.DelayFrame(1);
        ScreenFader.Instance.StartFade();
    }

}


