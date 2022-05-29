using System.Text;
using UnityEngine;
using UnityEngine.UI;
 
public class DebugWindow : MonoBehaviour
{
    public Text debugText;
    
    private void FixedUpdate () 
    {
        // Active holidays
        StringBuilder activeHolidays = new StringBuilder();
        /*HolidaysManager.ActiveHolidays
            .ForEach(holiday => activeHolidays.Append(holiday.name).Append(" "));*/

        debugText.text =
            $"Date: {Timeline.Time}\n" +
            $"Total minutes: {Timeline.TotalMinutes}\n" +
            $"Day minutes: {Timeline.CurrentMinute}\n" +
            $"Sun curve: {Timeline.SunCycleData.Today}\n";
        /* +
            $"Events: {activeHolidays}";*/
    }

}