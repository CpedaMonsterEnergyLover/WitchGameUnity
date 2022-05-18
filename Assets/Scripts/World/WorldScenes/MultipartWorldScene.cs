using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "WorldScenes/Multipart")]
public class MultipartWorldScene : WorldScene
{ 
    public int SubWorldsCount { get; set; } = 0;
    
    public override string GetFileName()
    {
        int currentWorldPartIndex = WorldPositionProvider.WorldIndex;
        StringBuilder sb = new StringBuilder().Append(sceneName);
        if (currentWorldPartIndex != -1) sb.Append("_").Append(currentWorldPartIndex);
        sb.Append(".json");
        return sb.ToString();
    }

    public override async UniTask LoadFromAnotherWorld(IWorldTransitionInitiator initiator, int worldPartIndex = -1)
    {
        WorldPositionProvider.WorldIndex = worldPartIndex;
        await base.LoadFromAnotherWorld(initiator, worldPartIndex);
    }
    
}


