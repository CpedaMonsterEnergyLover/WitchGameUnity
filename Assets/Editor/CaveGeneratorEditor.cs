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
            manager.DrawAllTiles();
            manager.DrawAllInteractable();
        }
        
        if (GUILayout.Button ("Clear all")) {
            manager.ClearAllTiles();
        }
    }
}