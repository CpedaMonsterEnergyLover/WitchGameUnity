using UnityEngine;

namespace WorldScenes
{
    [CreateAssetMenu(menuName = "WorldScenes/Solid")]
    public class SolidWorldScene : BaseWorldScene
    {
        [Header("Name of the world, which appears in the game")]
        public string worldName;
    }
}


