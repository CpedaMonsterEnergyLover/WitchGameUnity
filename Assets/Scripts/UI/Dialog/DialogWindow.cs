using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [Header("Parts of the window")] 
    public VerticalScrollAnimator[] animators = new VerticalScrollAnimator[3];
    public float animationDuration;
    public VolumeAnimator volumeAnimator;
    public DialogTree testDialog;
    public float textPrintSpeed;

    private DialogTree _dialogTree;
    private DialogElement _currentElement;
    private DialogPortrait[] _portraits;
    private DialogMember[] _members;
    // private int _currentElement = 0;
    private Coroutine _textRoutine;
    private bool _spaceBlocked;
    private string _finalText;
    private TemporaryDismissData _dismissData;

    protected override void OnEnable()
    {
        _dismissData = new TemporaryDismissData().Add(toDismiss).HideAll();
        base.OnEnable();
    }

    private void Update()
    {
        if(_dialogTree is null || _spaceBlocked) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Next();
        }
    }


    public void AnimateAll()
    {
        foreach (VerticalScrollAnimator animator in animators)
        {
            animator.Animate(animationDuration, false);
        }
        volumeAnimator.Animate(animationDuration, false);
        _spaceBlocked = true;
        Invoke(nameof(AllowSpace), animationDuration);
    }
    
    private void AllowSpace() => _spaceBlocked = false;

    public void StartDialog(DialogTree dialogTree)
    {
        if(isActiveAndEnabled) return;
        PlayerController.Instance.Stop();
        _dialogTree = dialogTree;
        gameObject.SetActive(true);
        _members = new[] {_dialogTree.leftSide, _dialogTree.rightSide};
        AnimateAll();
        if(dialogTree.leftSide is null) leftPortrait.SetMember(player);
        leftPortrait.SetMember(dialogTree.leftSide);
        rightPortrait.SetMember(dialogTree.rightSide);
        
        _currentElement = null;
        PlayElement(dialogTree.elements[0]);
    }

    private void EndDialog()
    {
        _dismissData = _dismissData.ShowAll();
        _dialogTree = null;
        foreach (VerticalScrollAnimator animator in animators) 
            animator.Animate(animationDuration, true);
        volumeAnimator.Animate(animationDuration, true);
        Invoke(nameof(Disable), animationDuration / 2);
  
    }
    
    private void Disable()
    {
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
            _portraits[(int) dialogSide].Select(selectionColor, true);
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
            yield return new WaitForSeconds(textPrintSpeed);
        }

        _textRoutine = null;
    }
    
}
