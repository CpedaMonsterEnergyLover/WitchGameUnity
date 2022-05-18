using UnityEngine;

namespace WorldScenes
{
    [CreateAssetMenu(menuName = "WorldScenes/Solid")]
    public class SolidWorldScene : WorldScene
    {
        [Header("Name of the world, which appears in the game")]
        public string worldName;

        public override string GetFileName() => $"{sceneName}.json";
    }
}


