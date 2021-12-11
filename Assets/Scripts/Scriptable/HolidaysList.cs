using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Season settings/Holidays list")]
public class HolidaysList : ScriptableObject
{
    [SerializeField]
    public List<Holiday> list;
}