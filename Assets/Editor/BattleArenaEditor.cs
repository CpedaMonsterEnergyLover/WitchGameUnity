using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (BattleArena))]
public class BattleArenaEditor : Editor {

    public override void OnInspectorGUI() {
        BattleArena manager = target as BattleArena;
        if (manager == null) return;

        DrawDefaultInspector();
        
        if (GUILayout.Button ("Clear tiles"))
        {
            manager.ClearTiles();
        }
        
        if (GUILayout.Button ("Start")) {
            manager.ClearTiles();
            manager.PaintArena();
        }
        
        if (GUILayout.Button ("Stop")) {
            manager.ClearArena();
        }
    }
}