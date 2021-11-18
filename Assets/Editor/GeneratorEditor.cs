using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (Generator))]
public class GeneratorEditor : Editor {

    public override void OnInspectorGUI() {
        Generator generator = target as Generator;
        if (generator == null) return;

        DrawDefaultInspector();
        
        if (GUILayout.Button ("Update")) {
            generator.ClearAllTiles();
            generator.GenerateWorld();
        }
        
        if (GUILayout.Button ("Clear all")) {
            generator.ClearAllTiles();
        }
    }
}