using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (GlobalVolumeAnimator))]
public class GlobalVolumeAnimatorEditor : Editor {

    public override void OnInspectorGUI() {
        GlobalVolumeAnimator manager = target as GlobalVolumeAnimator;
        if (manager == null) return;

        DrawDefaultInspector();
        
        if (GUILayout.Button ("Damage"))
        {
            manager.PlayDamage();
        }
        
        if (GUILayout.Button ("Death"))
        {
            manager.PlayDeath();
        }
        
        if (GUILayout.Button ("Clear"))
        {
            manager.Clear();
        }
    }
}