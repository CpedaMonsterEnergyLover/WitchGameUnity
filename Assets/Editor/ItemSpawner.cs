using Codice.Utils;
using UnityEditor;
using UnityEngine;

class ItemSpawner : EditorWindow
{
    private ItemData pickedItem;
    private int amount = 1;
    
    [MenuItem ("Window/Item spawner")]
    public static void  ShowWindow () {
        EditorWindow.GetWindow(typeof(ItemSpawner));
    }
    
    void OnGUI ()
    {
        amount = EditorGUILayout.IntField("Amount:", amount);
        if (GUILayout.Button("click")) {
            EditorGUIUtility.ShowObjectPicker<ItemData>(null, true, "", GUIUtility.GetControlID(FocusType.Passive) + 100);
        }

        if (Event.current.commandName == "ObjectSelectorClosed") {
            if (Application.isPlaying)
            {
                var item = (ItemData) EditorGUIUtility.GetObjectPickerObject();
                if (item is not null) Inventory.Instance.AddItem(item.identifier, amount);
            }
        }
    }
}