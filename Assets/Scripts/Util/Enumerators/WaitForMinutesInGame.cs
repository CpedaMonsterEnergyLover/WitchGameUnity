using UnityEngine;

public class WaitForMinutesInGame : CustomYieldInstruction
{
    private int _waitUntilTime = -1;
    
    public float WaitTime { get; }

    public override bool keepWaiting
    {
        get
        {
            if (_waitUntilTime < 0.0)
                _waitUntilTime = TimelineManager.minutesPassed + _waitUntilTime;
            bool flag = TimelineManager.minutesPassed < _waitUntilTime;
            if(!flag)
                Reset();
            return flag;
        }
    }

    public override void Reset() => _waitUntilTime = -1;
    
    public WaitForMinutesInGame(int time) => WaitTime = time;
}
