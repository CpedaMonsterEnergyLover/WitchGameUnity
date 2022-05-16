using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "WorldScenes/Multipart")]
public class MultipartWorldScene : BaseWorldScene
{
    [Header("IWorldPartDataProvider")]
    public List<ScriptableObject> worldParts = new ();

    public new string FileName
    {
        get
        {
            int currentWorldPartIndex = WorldTransitionDataProvider.WorldIndex;
            StringBuilder sb = new StringBuilder().Append(sceneName);
            if (currentWorldPartIndex != -1) sb.Append("_").Append(currentWorldPartIndex);
            sb.Append(".json");
            return sb.ToString();
        }
    }

    public override void LoadFromAnotherWorld(int worldPartIndex = -1)
    {
        if (worldPartIndex == -1 || worldPartIndex > worldParts.Count)
        {
            Debug.LogError($"Invalid world part index: {worldPartIndex}. This world has {worldParts.Count} parts");
            return;
        }

        if (worldParts[worldPartIndex] is IWorldTransitionData provider)
        {
            WorldTransitionDataProvider.WorldTransitionData = provider;
            WorldTransitionDataProvider.WorldIndex = worldPartIndex;
        }
        else
        {
            Debug.LogError($"World part with index {worldPartIndex} is not implementing IWorldPartData");
            return;
        }
        base.LoadFromAnotherWorld(worldPartIndex);
    }
}


