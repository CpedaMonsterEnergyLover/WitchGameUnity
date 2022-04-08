using UnityEngine;

namespace WorldScenes
{
    [CreateAssetMenu(menuName = "WorldScenes/Multipart")]
    public class MultipartWorldScene : BaseWorldScene
    {
        public override void Load(int subworldIndex = -1)
        {
            if (subworldIndex == -1)
            {
                Debug.LogError("Invalid subworld index");
                return;
            }

            GameDataManager.Instance.CurrentSubWorldIndex = subworldIndex;
            base.Load(subworldIndex);
        }
    }
}


