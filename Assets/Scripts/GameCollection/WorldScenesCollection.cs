using System.Collections.Generic;
using UnityEngine;
using WorldScenes;

public class WorldScenesCollection : MonoBehaviour
{
    public List<BaseWorldScene> scenes;

    private static readonly Dictionary<string, BaseWorldScene> Collection = new();
    public static BaseWorldScene Get(string id) => Collection[id];

        
    public void Init()
    {
        scenes.Clear();
        scenes.ForEach(scene => Collection.Add(scene.sceneName, scene));
    }
}