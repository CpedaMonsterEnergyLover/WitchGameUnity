using UnityEngine;
using UnityEngine.SceneManagement;

namespace WorldScenes
{
    [System.Serializable]
    public abstract class BaseWorldScene : ScriptableObject
    {
        [Header("Name of the scene, which this world loads")]
        public string sceneName;
        [Header("Name of the world, which appears in the game")]
        public string worldName;
        [Header("Does this world has sun?")]
        public bool hasGlobalIllumination;

        public string FileName => $"{sceneName}.json"; 

        public virtual void LoadFromAnotherWorld(int subworldIndex = -1)
        {
            GameDataManager.SaveTemporaryWorldData();
            SceneManager.LoadScene(sceneName);
        }

        public virtual void LoadFromMainMenu()
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}