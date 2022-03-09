using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (WorldManager))]
public class GeneratorEditor : Editor {

    public override void OnInspectorGUI() {
        WorldManager manager = target as WorldManager;
        if (manager == null) return;

        DrawDefaultInspector();
        
        if (GUILayout.Button ("Update"))
        {
            manager.ClearAllInteractable();
            manager.GenerateWorld();
            manager.DrawAllTiles();
        }
        
        if (GUILayout.Button ("Clear all")) {
            manager.ClearAllTiles();
        }
    }
}