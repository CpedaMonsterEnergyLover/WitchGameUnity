using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Sun curve")]
public class SunCurve : ScriptableObject
{
    [SerializeField]
    public AnimationCurve curve;
}
