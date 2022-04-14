using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoadingBar : MonoBehaviour
{
    public GameObject toDisable;
    public Image loadingBar;
    public Text loadingText;
    private float _phaseLen;
    private float _phasesAmount;
    private string _dots = "";    
    private float _target;
    private float _current;
    private static int _phaseCounter;
    
    
    public void Activate(int phasesAmouns)
    {
        toDisable.SetActive(false);
        _phasesAmount = phasesAmouns;
        _phaseLen = 1f / phasesAmouns;
        loadingText.text = "";
        _current = 0;
        _target = 0;
        gameObject.SetActive(true);
        _phaseCounter = 1;
        StartCoroutine(NameDotsRoutine());
    }

    public void SetPhase(string phaseName)
    {
        _target = _phaseLen * _phaseCounter;
        _phaseCounter++;
        SetName(phaseName);
    }
    
    private void SetName(string phaseName) => loadingText.text = phaseName + _dots;

    private void OnDisable()
    {
        StopAllCoroutines();
        loadingBar.fillAmount = 0f;
        _current = 0;
        _target = 0;
    }



    private void Update()
    {
        _current = Mathf.MoveTowards(_current, _target, Time.deltaTime);
        loadingBar.fillAmount = _current;
    }

    private IEnumerator NameDotsRoutine()
    {
        string phaseText = loadingText.text;
        
        while (gameObject.activeSelf)
        {
            if (_dots.Length > 3) _dots = "";
            else _dots += ". ";
            loadingText.text = phaseText + _dots;
            yield return new WaitForSeconds(1f);
        }
    }
}

