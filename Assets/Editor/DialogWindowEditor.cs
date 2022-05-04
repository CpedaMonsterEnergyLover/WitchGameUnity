using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (DialogWindow))]
public class DialogWindowEditor : UnityEditor.Editor
{

    
    public override void OnInspectorGUI() {
        DialogWindow manager = target as DialogWindow;
        if (manager == null) return;

        DrawDefaultInspector();
        
        if (GUILayout.Button ("Start test"))
        {
            manager.StartDialog(manager.testDialog);
        }
        
        if (GUILayout.Button ("Next"))
        {
            manager.Next();
        }
        
    }
}