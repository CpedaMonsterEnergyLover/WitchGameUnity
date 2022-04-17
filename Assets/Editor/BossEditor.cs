using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (Boss))]
public class BossEditor : Editor {

    public override void OnInspectorGUI() {
        Boss manager = target as Boss;
        if (manager == null) return;

        DrawDefaultInspector();
        
        if (GUILayout.Button ("Apply 1 damage"))
        {
            manager.ApplyDamage(1);
        }
        
        if (GUILayout.Button ("Apply 3 damage"))
        {
            manager.ApplyDamage(1);
        }
    }
}