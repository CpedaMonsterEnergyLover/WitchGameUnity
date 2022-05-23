using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (GameSystemLoader))]
public class GameSystemLoaderEditor : Editor {

    public override void OnInspectorGUI() {
        GameSystemLoader manager = target as GameSystemLoader;
        if (manager == null) return;

        DrawDefaultInspector();
        
        if (GUILayout.Button ("Create game system"))
        {
            manager.CreateGameSystem();
        }
        
        if (GUILayout.Button ("Destroy game system")) {
            manager.DestroyGameSystem();
        }  
        
        if (GUILayout.Button ("Save game data") && Application.isPlaying)
        {
            GameDataManager.SaveAll();
        }
        
        if (GUILayout.Button ("Delete all game data")) {
            GameDataManager.InitDirPaths();
            GameDataManager.ClearAllData();
        }      
    }
}