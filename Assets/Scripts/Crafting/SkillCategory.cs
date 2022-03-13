using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Category")]
public class SkillCategory : ScriptableObject
{
    public string title;
    public byte value;
    public Sprite icon;

    public bool Is(SkillCategory right)
    {
        return value == right.value;
    }

}