using UnityEngine;

[CreateAssetMenu(menuName = "Dialogs/Member")]
public class DialogMember : ScriptableObject
{
    public new string name;
    public Sprite portraitImage;
    public bool hasDecoration;
    public GameObject decoration;
}
