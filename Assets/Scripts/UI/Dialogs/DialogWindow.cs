using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogWindow : BaseWindow
{
    public static DialogWindow Instance;
    public override void Init()
    {
        Instance = this;
        _portraits = new[]
        {
            leftPortrait,
            rightPortrait
        };
    }

    [Header("ITemporaryDismissable")] 
    public List<Component> toDismiss = new();
    public Color selectionColor;
    public DialogPortrait leftPortrait;
    public DialogPortrait rightPortrait;
    public Text dialogText;
    [Header("Player")]
    public DialogMember player;

    [Header("Animators")] 
    public Animator topLitterBoxAnimator;
    public Animator bottomLitterBoxAnimator;
    public Animator vignetteAnimator;
    public Animator panelAnimator;
    public GameObject skipIndicator;
    public DialogTree testDialog;
    public float textPrintSpeed;

    private DialogTree _dialogTree;
    private DialogElement _currentElement;
    private DialogPortrait[] _portraits;
    private DialogMember[] _members;
    // private int _currentElement = 0;
    private Coroutine _textRoutine;
    private string _finalText;
    private TemporaryDismissData _dismissData;
    private Coroutine _keyListenerRoutine;

    protected override void OnEnable()
    {
        _dismissData = new TemporaryDismissData().Add(toDismiss).HideAll();
        base.OnEnable();
    }
    
    private IEnumerator KeyListenerRoutine()
    {
        while (isActiveAndEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Space) ||
                Input.GetMouseButtonDown(0) || 
                Input.GetMouseButtonDown(1))
                Next();
            yield return null;
        }
    }
    
    private void AnimateStop()
    {
        topLitterBoxAnimator.Play("TopLitterBoxStop");
        bottomLitterBoxAnimator.Play("BottomLitterBoxStop");
        vignetteAnimator.Play("VignetteStop");
        panelAnimator.Play("DialogPanelStop");
    }
    
    public void StartDialog(DialogTree dialogTree)
    {
        if(isActiveAndEnabled) return;
        
        PlayerController.Instance.Stop();
        _dialogTree = dialogTree;
        _members = new[] {_dialogTree.leftSide, _dialogTree.rightSide};
        if(dialogTree.leftSide is null) leftPortrait.SetMember(player);
        leftPortrait.SetMember(dialogTree.leftSide);
        rightPortrait.SetMember(dialogTree.rightSide);
        _currentElement = null;
        dialogText.text = string.Empty;
        gameObject.SetActive(true);
        Time.timeScale = 0;
        StartCoroutine(PlayDialog());
    }

    private IEnumerator PlayDialog()
    {
        yield return new WaitForSecondsRealtime(1f);
        _keyListenerRoutine = StartCoroutine(KeyListenerRoutine());
        PlayElement(_dialogTree.elements[0]);
    }

    private void EndDialog()
    {
        AnimateStop();
        StopCoroutine(_keyListenerRoutine);
        _keyListenerRoutine = null;
        StartCoroutine(Disable());
    }
    
    private IEnumerator Disable()
    {
        yield return new WaitForSecondsRealtime(0.95f);
        Time.timeScale = 1;
        _dismissData = _dismissData.ShowAll();
        _dialogTree = null;
        gameObject.SetActive(false);
    }

    public void Next()
    {
        if (_textRoutine is not null)
        {
            StopCoroutine(_textRoutine);
            dialogText.text = _finalText;
            _textRoutine = null;
        }
        else
        {
            int currentIndex = _dialogTree.elements.IndexOf(_currentElement);
            if(currentIndex < _dialogTree.elements.Count - 1)
                PlayElement(_dialogTree.elements[currentIndex + 1]);
            else
                EndDialog();
        }
    }

    private void SelectSide(DialogSide dialogSide)
    {
        if (_currentElement is null)
        {
            _portraits[(int) dialogSide].Select(selectionColor);
            return;
        }
        if (dialogSide == _currentElement.speakingSide) return;
        _portraits[(int) _currentElement.speakingSide].Unselect();
        _portraits[(int) dialogSide].Select(selectionColor);
    }

    private void PlayElement(DialogElement element)
    {
        SelectSide(element.speakingSide);
        _currentElement = element;
        _textRoutine = StartCoroutine(TextPrintRoutine(element.SpeakerName(_members), element.text));
    }

    private IEnumerator TextPrintRoutine(string speakerName, string text)
    {
        skipIndicator.SetActive(false);
        speakerName += ": ";
        _finalText = speakerName + text;
        string printed = "";
        int finalLen = text.Length;
        int index = 0;
        while (index < finalLen)
        {
            printed += text[index];
            dialogText.text = speakerName + printed;
            index++;
            yield return new WaitForSecondsRealtime(textPrintSpeed);
        }
        
        skipIndicator.SetActive(true);
        _textRoutine = null;
    }
    
}
