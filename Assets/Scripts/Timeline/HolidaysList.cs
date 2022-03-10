using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Holidays list")]
public class HolidaysList : ScriptableObject
{
    [SerializeField]
    public List<Holiday> list;
}