using DefaultNamespace;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (GameSystem))]
public class GameSystemEditor : UnityEditor.Editor {

    public override void OnInspectorGUI() {
        GameSystem manager = target as GameSystem;
        if (manager == null) return;

        DrawDefaultInspector();
        
        if (GUILayout.Button ("Save all"))
        {
            manager.collectionManager.Init();
            GameDataManager.SaveAll();
        }
        
        if (GUILayout.Button ("Delete all data")) {
            GameDataManager.ClearPers();
            GameDataManager.ClearTemp();
        }
    }
}