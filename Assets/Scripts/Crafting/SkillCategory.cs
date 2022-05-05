using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Category")]
public class SkillCategory : ScriptableObject
{
    public string title;
    public byte value;
    public Sprite icon;
}