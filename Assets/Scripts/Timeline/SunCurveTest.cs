using System.Collections.Generic;
using UnityEngine;

public class SunCurveTest : MonoBehaviour
{
    public SunCurve sunCurve;
    public int transitionDuration;
    public int yearLen;
    public int seasonLen;

    private readonly List<Vector2> sunsets = new ();
    private readonly List<Vector2> sunrises = new ();

    public void Visualise()
    {
        SunCycleData sunCycleData = new SunCycleData(sunCurve, transitionDuration, yearLen);
        for (int month = 0; month < 12; month++)
        for (int day = 0; day < seasonLen; day++)
        {
            var dayCycle = sunCycleData.GetDay(month, day, seasonLen);
            int x = month * seasonLen + day;
            sunrises.Add(new Vector2(x, dayCycle.Sunrise / 30f));
            sunsets.Add(new Vector2(x, dayCycle.Sunset / 30f));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        foreach (Vector2 v in sunrises) Gizmos.DrawSphere(v, 0.2f);
        Gizmos.color = Color.blue;
        foreach (Vector2 v in sunsets) Gizmos.DrawSphere(v, 0.2f);
    }
}
