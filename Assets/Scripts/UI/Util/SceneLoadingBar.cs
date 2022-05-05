using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoadingBar : MonoBehaviour
{
    public GameObject toDisable;
    public Image loadingBar;
    public Text loadingText;
    private float _phaseLen;
    private string _dots = "";    
    private float _target;
    private float _current;
    private string _phaseName;
    private static int _phaseCounter;

    private Coroutine _routine;

    public void Activate(int phases)
    {
        toDisable.SetActive(false);
        _phaseLen = 1f / phases;
        loadingText.text = "Сбор данных";
        loadingBar.fillAmount = 0;
        _current = 0;
        _target = 0;
        _phaseCounter = 1;
        gameObject.SetActive(true);
    }

    public void SetPhase(string phaseName)
    {
        _target = _phaseLen * _phaseCounter;
        _phaseCounter++;
        _phaseName = phaseName;
        UpdateName();
        if(_routine is null) _routine = StartCoroutine(NameDotsRoutine());
    }


    private void UpdateName() 
        => loadingText.text = new StringBuilder().Append(_phaseName).Append(_dots).ToString();

    private void OnDisable()
    {
        StopAllCoroutines();
        loadingBar.fillAmount = 0f;
        _current = 0;
        _target = 0;
    }
    
    private void FixedUpdate()
    {
        _current = Mathf.MoveTowards(_current, _target,   Time.deltaTime);
        loadingBar.fillAmount = _current;
    }

    private IEnumerator NameDotsRoutine()
    {
        while (gameObject.activeSelf)
        {
            if (_dots.Length > 3) _dots = "";
            else _dots += ".";
            UpdateName();
            yield return new WaitForSecondsRealtime(0.6f);
        }
    }
}

