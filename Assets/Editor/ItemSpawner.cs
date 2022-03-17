using UnityEditor;
using UnityEngine;

class ItemSpawner : EditorWindow
{
    private ItemData pickedItem;
    private int amount = 1;
    private bool added;
    
    [MenuItem ("Window/Item spawner")]
    public static void  ShowWindow () {
        EditorWindow.GetWindow(typeof(ItemSpawner));
    }
    
    void OnGUI ()
    {
        amount = EditorGUILayout.IntField("Amount:", amount);
        if (GUILayout.Button("click")) {
            EditorGUIUtility.ShowObjectPicker<ItemData>(null, true, "", GUIUtility.GetControlID(FocusType.Passive) + 100);
            added = false;
        }

        if (Event.current.commandName == "ObjectSelectorClosed") {
            if (Application.isPlaying)
            {
                var item = (ItemData) EditorGUIUtility.GetObjectPickerObject();
                if (item is not null && !added)
                {
                    added = true;
                    WindowManager.Get<InventoryWindow>(WindowIdentifier.Inventory)
                        .AddItem(item.identifier, amount);
                }
            }
        }
    }
}