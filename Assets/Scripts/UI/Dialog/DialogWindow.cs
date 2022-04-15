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

    public Color selectionColor;
    public DialogPortrait leftPortrait;
    public DialogPortrait rightPortrait;
    public Text dialogText;
    
    private DialogTree _dialogTree;
    private DialogElement _currentElement;
    private DialogPortrait[] _portraits;

    protected override void OnEnable()
    {
        // Hide ITemporaryDismissable
        base.OnEnable();
    }

    public void StartDialog(DialogTree dialogTree)
    {
        _dialogTree = dialogTree;
        leftPortrait.SetMember(dialogTree.leftSide);
        rightPortrait.SetMember(dialogTree.rightSide);
        PlayElement(dialogTree.dialogElement);
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
        dialogText.text = element.text;
    }
    
}
