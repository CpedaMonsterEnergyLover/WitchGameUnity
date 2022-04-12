using UnityEngine;
using UnityEngine.UI;

public class Moon : MonoBehaviour
{
    public Sprite[] moonStages = new Sprite[8];
    public Image moonImage;

    private int _activeStage;
    
    private void Start()
    {
        SetStage(0);
    }

    private void SetStage(int stage)
    {
        if (stage > 7) stage = 0;
        _activeStage = stage;
        moonImage.sprite = moonStages[stage];
    }

    public void ToggleStage()
    {
        SetStage(_activeStage + 1);
    }
}
