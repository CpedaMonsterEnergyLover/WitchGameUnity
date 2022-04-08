using UnityEngine;
using UnityEngine.SceneManagement;

namespace WorldScenes
{
    public abstract class Base : ScriptableObject
    {
        [Header("Name of the scene, which this world loads")]
        public string sceneName;
        [Header("Name of the world, which appears in the game")]
        public string worldName;
        
        public string GetPersistantPath() => $"worldscenes/persistant/{worldName}"; 
        public string GetTemporaryPath() => $"worldscenes/temporary/{worldName}";

        public virtual void Load(int subworldIndex = -1)
        {
            // Save current world temporary data
            GameDataManager.Instance.SaveTemporaryWorldData(WorldManager.Instance.BuildTemporaryData());
            // Load new scene
            SceneManager.LoadScene(sceneName);
        }
    }
}