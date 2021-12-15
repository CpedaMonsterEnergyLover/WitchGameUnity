using Codice.Utils;
using UnityEditor;
using UnityEngine;

class ItemSpawner : EditorWindow
{
    private Item pickedItem;
    
    [MenuItem ("Window/Item spawner")]
    public static void  ShowWindow () {
        EditorWindow.GetWindow(typeof(ItemSpawner));
    }
    
    void OnGUI ()
    {
        if (GUILayout.Button("click")) {
            EditorGUIUtility.ShowObjectPicker<Item>(null, true, "", GUIUtility.GetControlID(FocusType.Passive) + 100);
        }

        if (Event.current.commandName == "ObjectSelectorClosed") {
            if (Application.isPlaying)
            {
                var item = (Item) EditorGUIUtility.GetObjectPickerObject();
                if (item is not null) Inventory.Instance.AddItem(item, 1);
            }
        }
    }
}