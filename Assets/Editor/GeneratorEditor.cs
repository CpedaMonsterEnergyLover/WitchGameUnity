using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (WorldManager))]
public class GeneratorEditor : Editor {

    public override void OnInspectorGUI() {
        WorldManager worldManager = target as WorldManager;
        if (worldManager == null) return;

        DrawDefaultInspector();
        
        if (GUILayout.Button ("Update"))
        {
            worldManager.Generate();
            worldManager.DrawAllTiles();
        }
        
        if (GUILayout.Button ("Clear all")) {
            worldManager.ClearAllTiles();
        }
    }
}