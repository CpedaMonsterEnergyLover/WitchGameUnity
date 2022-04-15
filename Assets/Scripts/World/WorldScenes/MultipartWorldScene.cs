using UnityEngine;

namespace WorldScenes
{
    [CreateAssetMenu(menuName = "WorldScenes/Multipart")]
    public class MultipartWorldScene : BaseWorldScene
    {
        public override void LoadFromAnotherWorld(int subworldIndex = -1)
        {
            if (subworldIndex == -1)
            {
                Debug.LogError("Invalid subworld index");
                return;
            }

            base.LoadFromAnotherWorld(subworldIndex);
        }
    }
}


