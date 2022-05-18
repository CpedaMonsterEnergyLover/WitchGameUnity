using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WorldScenes;

public class WorldScenesCollection : MonoBehaviour
{
    public List<WorldScene> scenes;
    public static List<string> MultipartSceneNames { get; private set; }

    private static readonly Dictionary<string, WorldScene> Collection = new();
    public static WorldScene Get(string id) => Collection.ContainsKey(id) ? Collection[id] : null;
        
    public void Init()
    {
        Collection.Clear();
        scenes.ForEach(scene => Collection.Add(scene.sceneName, scene));
        MultipartSceneNames = new List<string>();
        foreach (WorldScene scene in scenes)
            if (scene is MultipartWorldScene multipartWorldScene)
            {
                multipartWorldScene.SubWorldsCount = 0;
                MultipartSceneNames.Add(multipartWorldScene.sceneName);
            }
    }
}