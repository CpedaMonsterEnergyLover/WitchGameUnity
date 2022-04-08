using UnityEngine;

namespace WorldScenes
{
    public class TestingSceneLoader : MonoBehaviour, IPlayerReceiver
    {
        public BaseWorldScene sceneToLoad;
        
        public void OnReceivePlayer()
        {
            sceneToLoad.Load();
        }

        public void OnPlayerExitReceiver()
        { }
    }
}