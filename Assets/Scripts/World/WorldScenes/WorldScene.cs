using UnityEngine;
using UnityEngine.SceneManagement;

    [System.Serializable]
    public abstract class BaseWorldScene : ScriptableObject
    {
        [Header("Name of the scene, which this world loads")]
        public string sceneName;
        [Header("Does this world has global illumination?")]
        public bool hasGlobalIllumination;

        public string FileName => $"{sceneName}.json"; 

        public virtual void LoadFromAnotherWorld(int worldPartIndex = -1)
        {
            GameDataManager.SaveTemporaryWorldData();
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }

        public virtual void LoadFromMainMenu()
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }
