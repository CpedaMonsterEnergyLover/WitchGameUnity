using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (WorldManager))]
public class GeneratorEditor : UnityEditor.Editor {

    public override async void OnInspectorGUI() {
        WorldManager manager = target as WorldManager;
        if (manager == null) return;

        DrawDefaultInspector();
        
        
        if (GUILayout.Button ("Clear all")) {
            manager.ClearAllTiles();
        }
        
        if (GUILayout.Button ("Update"))
        {
            manager.ClearAllInteractable();
            await manager.GenerateWorld();
            manager.DrawAllTiles();
            manager.DrawAllInteractable();
        }
    }
}
