using System.Collections.Generic;
using UnityEngine;
using WorldScenes;

public class WorldScenesCollection : MonoBehaviour
{
    public List<BaseWorldScene> scenes;

    private static readonly Dictionary<string, BaseWorldScene> Collection = new();
    public static BaseWorldScene Get(string id) => Collection.ContainsKey(id) ? Collection[id] : null;

        
    public void Init()
    {
        Collection.Clear();
        scenes.ForEach(scene => Collection.Add(scene.sceneName, scene));
    }
}