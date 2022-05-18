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

    [Header("Loading bar")] 
    public WorldScene sceneToLoad;
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
            if (!_needsConfirm)
            {
                confirmRewriteText.gameObject.SetActive(true);
                _needsConfirm = true;
            }
            else
                LoadNewWorldScene().Forget();
        else
            LoadNewWorldScene().Forget();
    }
    
    private async UniTask LoadNewWorldScene()
    {
        screenFader.PlayTransparent();
        _needsConfirm = false;
        GameDataManager.ClearAllData();
        
        WorldSettingsProvider.Clear();
        WorldSettingsProvider.SetSettings(new WorldSettings(
            difficultyToggleGroup.value,
            worldSizeToggleGroup.value,
            seedInput.text,
            seasonSliderController.Value));
        
        await screenFader.StartFade(0.5f);
        sceneToLoad.LoadFromMainMenu();
    }

}


