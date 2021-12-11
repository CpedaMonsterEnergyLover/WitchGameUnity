using UnityEngine;

[CreateAssetMenu(menuName = "Season settings/Sun curve")]
public class SunCurve : ScriptableObject
{
    [SerializeField]
    public AnimationCurve curve;
}
