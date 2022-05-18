using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WorldScenes
{
    public class TestingSceneLoader : MonoBehaviour, IPlayerReceiver
    {
        public WorldScene sceneToLoad;
        
        public void OnReceivePlayer()
        {
            // sceneToLoad.LoadFromAnotherWorld().Forget();
        }

        public void OnPlayerExitReceiver()
        { }
    }
}