using DefaultNamespace;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (GameSystem))]
public class GameSystemEditor : UnityEditor.Editor {

    public override void OnInspectorGUI() {
        GameSystem manager = target as GameSystem;
        if (manager == null) return;

        DrawDefaultInspector();
        
        if (GUILayout.Button ("Save all") && Application.isPlaying)
        {
            manager.collectionManager.Init();
            GameDataManager.SaveAll(null);
        }
        
        if (GUILayout.Button ("Delete all data")) {
            GameDataManager.InitDirPaths();
            GameDataManager.ClearAllData();
        }
    }
}