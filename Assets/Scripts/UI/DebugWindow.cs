using System.Text;
using UnityEngine;
using UnityEngine.UI;
 
public class DebugWindow : MonoBehaviour
{
    public WorldManager world;
    public Text modeText;
    public Text debugText;

    private float _deltaTime;
    private bool _debugActive;

    private void FixedUpdate () {

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (_debugActive) DisableDebug();
            else EnableDebug();
        }
        
        if (_debugActive)
        {
            // FPS
            _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
            float fps = 1.0f / _deltaTime;
            
            // Active holidays
            StringBuilder activeHolidays = new StringBuilder();
            HolidaysManager.ActiveHolidays.ForEach(delegate(Holiday holiday)
            {
                activeHolidays.Append(holiday.name).Append(" ");
            });
            
            /*// Update text
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
                .Append(world.CurrentLoadedTilesAmount)
                .Append("\nCached objects: ")
                .Append(world.CurrentCacheSize)
                .Append("/")
                .Append(world.tileCacheSize)
                // Timeline data
                .Append("\nDate: ").Append(TimelineManager.time)
                .Append("\nTotal hours: ").Append(TimelineManager.TotalHours)
                .Append("\nSun curve: ")
                .Append(SunCycleManager.TodaysSunCurve)
                .Append("\nIs sun down: ")
                .Append(SunCycleManager.IsSunDownCached)
                .Append("\nActive event: ")
                .Append(activeHolidays)
                .Append("\nCursor Mode: ").Append(CursorManager.Instance.Mode)
                /*.Append("\nPicked Item: ")
                .Append(ItemPicker.Instance.itemSlot.HasItem ? ItemPicker.Instance.itemSlot.storedAmount : "0").Append(" of ")
                .Append(ItemPicker.Instance.itemSlot.HasItem ? ItemPicker.Instance.Item.Data.name : "nothing")
                .Append("\nPicker preview active: ").Append(ItemPicker.Instance._previewActive)#1#
                .ToString();*/
        }
        
    }
    
    void Start()
    {
        modeText.enabled = true;
        EnableDebug();
    }

    void EnableDebug()
    {
        if (!_debugActive)
        {
            modeText.text = "Debug mode ON";
            debugText.enabled = true;
            _debugActive = true;
        }
    }

    void DisableDebug()
    {
        if (_debugActive)
        {
            modeText.text = "Press F1 to enable debug";
            debugText.enabled = false;
            _debugActive = false;
        }
    }
}