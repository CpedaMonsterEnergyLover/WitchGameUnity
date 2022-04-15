using UnityEngine;

[CreateAssetMenu(menuName = "Dialogs/Tree")]
public class DialogTree : ScriptableObject
{
    public DialogMember leftSide;
    public DialogMember rightSide;
    public DialogElement dialogElement;
}

