using System.Text;
using UnityEngine;
using UnityEngine.UI;
 
public class DebugWindow : MonoBehaviour
{
    public WorldManager world;
    public Text modeText;
    public Text debugText;

    private float deltaTime;
    private bool debugActive;

    private void Update () {

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (debugActive) DisableDebug();
            else EnableDebug();
        }
        
        if (debugActive)
        {
            // FPS
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            
            // Update text
            debugText.text = new StringBuilder()
                // FPS
                .Append(fps.ToString("0"))
                .Append("/")
                .Append(world.targetFrameRate)
                .Append(" FPS")
                // Player coordinates
                .Append("\nPlayer: ")
                .Append(Vector3Int.FloorToInt(world.playerTransform.position))
                // View distance, chunks loaded, chunks to load
                .Append("\nView distance: X: ")
                .Append(world.viewRangeX).Append(", Y: ").Append(world.viewRangeY)
                .Append("\nChunks loaded: ")
                .Append(world.loadedTiles.Count)
                .Append("\nCached objects: ")
                .Append(world.tileCache.Count)
                .Append("/")
                .Append(world.tileCacheSize)
                // Timeline data
                /*.Append("\nDate: ").Append(Timeline.time)
                .Append("\nTotal hours: ").Append(Timeline.TotalHours)
                .Append("\nSun curve: ")
                .Append(SunCycleManager.TodaysSunCurve)
                .Append("\nIs sun down: ")
                .Append(SunCycleManager.IsSunDownCached)
                .Append("\nActive event: ")
                .Append(ActiveHolidays)
                .Append(HolidaysManager.IsHolidayActive(1))*/
                .ToString();
        }
        
    }
    
    void Start()
    {
        modeText.enabled = true;
        EnableDebug();
    }

    void EnableDebug()
    {
        if (!debugActive)
        {
            modeText.text = "Debug mode ON";
            debugText.enabled = true;
            debugActive = true;
        }
    }

    void DisableDebug()
    {
        if (debugActive)
        {
            modeText.text = "Press F1 to enable debug";
            debugText.enabled = false;
            debugActive = false;
        }
    }
}