using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (ParticleSystemIndividualSortingOrder))]
public class ParticleSystemIndividualSortingOrderEditor : Editor {

    public override void OnInspectorGUI() {
        ParticleSystemIndividualSortingOrder manager = target as ParticleSystemIndividualSortingOrder;
        if (manager == null) return;

        DrawDefaultInspector();
        

        
        if (GUILayout.Button ("Recreate pool")) {
            manager.RecreatePool();
        }
    }
}


