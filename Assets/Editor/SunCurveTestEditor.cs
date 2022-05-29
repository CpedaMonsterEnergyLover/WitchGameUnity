using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (SunCurveTest))]
public class SunCurveTestEditor : Editor {

    public override void OnInspectorGUI() {
        SunCurveTest manager = target as SunCurveTest;
        if (manager == null) return;

        DrawDefaultInspector();
        
        if (GUILayout.Button ("Visualize"))
        {
            manager.Visualise();
        }
    }
}