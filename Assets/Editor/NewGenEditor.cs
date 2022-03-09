using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (NewGen.WorldManager))]
public class NewGenEditor : Editor {

    public override void OnInspectorGUI() {
        NewGen.WorldManager manager = target as NewGen.WorldManager;
        if (manager == null) return;

        DrawDefaultInspector();
        
        if (GUILayout.Button ("Update"))
        {
            // manager.ClearAllTiles();
            manager.GenerateWorld();
            manager.DrawAllTiles();
            // manager.DrawAllTiles();
        }
        
        if (GUILayout.Button ("Clear all")) {
            manager.ClearAllTiles();
        }
    }
}