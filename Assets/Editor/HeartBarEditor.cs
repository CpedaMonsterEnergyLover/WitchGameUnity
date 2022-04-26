using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (HeartBar))]
public class HeartBarEditor : Editor {

    public override void OnInspectorGUI() {
        HeartBar manager = target as HeartBar;
        if (manager == null) return;

        DrawDefaultInspector();
        
        if (GUILayout.Button ("Add heart"))
        {
            manager.AddHeart(manager.heartDataToAdd, manager.heartTypeToAdd);
        }
        
        if (GUILayout.Button ("Apply damage"))
        {
            manager.ApplyDamage(DamageType.Physical);
        }
        
        if (GUILayout.Button ("Convert"))
        {
            manager.ConvertHeart(manager.indexToConvert, manager.heartDataToAdd.origin);
        }
        
    }
}