using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

public static class PrettyDebug
{
    private static MonoBehaviour _latestObject;
    private static string _currentColor;
    
    public static void Log(string message, MonoBehaviour context)
    {
        UpdateColor(context);
        StringBuilder sb = new StringBuilder()
            .Append(_currentColor)
            .Append("[")
            .Append(context.gameObject.name)
            .Append("]")
            .Append("</color>")
            .Append(" ").Append(message);
        _latestObject = context;
        Debug.Log(sb.ToString(), context);
    }

    private static void UpdateColor(Object context)
    {
        if (context == _latestObject) return;
        _currentColor = context switch
        {
            GameSystemLoader or GameSystem => "<color=red>",
            PlayerController or PlayerManager => "<color=green>",
            _ => "<color=white>"
        };
    }
}