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
        HolidaysManager.ActiveHolidays
            .ForEach(holiday => activeHolidays.Append(holiday.name).Append(" "));

        debugText.text = 
            $"Date: {TimelineManager.time}\n" +
            $"Total hours: {TimelineManager.totalHours}\n" +
            $"Sun curve: {SunCycleManager.TodaysSunCurve}\n" +
            $"Sun is down: {SunCycleManager.IsSunDownCached}\n" +
            $"Events: {activeHolidays}";
    }

}