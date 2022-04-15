using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (ScreenFader))]
public class ScreenFaderEditor : Editor {

    public override void OnInspectorGUI() {
        ScreenFader fader = target as ScreenFader;
        if (fader == null) return;

        DrawDefaultInspector();
        
        if (GUILayout.Button ("Fade in scaled"))
        {
            fader.FadeScaled(true);
        }
        
        if (GUILayout.Button ("Fade out scaled")) {
            fader.FadeScaled(false);
        }
        
        if (GUILayout.Button ("Fade in unscaled"))
        {
            fader.FadeUnscaled(true);
        }
        
        if (GUILayout.Button ("Fade out unscaled")) {
            fader.FadeUnscaled(false);
        }
    }
}