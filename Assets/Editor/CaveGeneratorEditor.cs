using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (CaveManager))]
public class CaveGeneratorEditor : Editor {

    public override void OnInspectorGUI() {
        CaveManager manager = target as CaveManager;
        if (manager == null) return;

        DrawDefaultInspector();
        
        if (GUILayout.Button ("Update"))
        {
            manager.ClearAllInteractable();
            manager.GenerateWorld();
        }
        
        if (GUILayout.Button ("Clear all")) {
            manager.ClearAllTiles();
        }
    }
}