using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (HeartContainer))]
public class HeartBarEditor : Editor {

    public override void OnInspectorGUI() {
        HeartContainer manager = target as HeartContainer;
        if (manager == null) return;

        DrawDefaultInspector();
        
        if (GUILayout.Button ("Add heart"))
        {
            Heart toAdd = Heart.Create(manager.origin, manager.type);
            manager.Add(toAdd);
        }
        
        if (GUILayout.Button ("Apply damage"))
        {
            manager.ApplyDamage(manager.damageToApply);
        }
        
        if (GUILayout.Button ("Change origin"))
        {
            manager.ChangeOrigin(manager.indexToPop, manager.origin);
        }
        
        if (GUILayout.Button ("Pop"))
        {
            manager.Pop(manager.indexToPop);
        }
        
        /*if (GUILayout.Button ("Convert"))
        {
            // manager.ConvertHeart(manager.indexToConvert, manager.heartDataToAdd.origin);
        }*/
        
    }
}