using System.Collections.Generic;
using UnityEngine;
using WorldScenes;

public class GameDataManager : MonoBehaviour
{
    #region Singleton

    public static GameDataManager Instance;
    
    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public class TemporaryWorldData
    {
        public TemporaryWorldData(List<WorldTile> data, Base worldScene)
        {
            Data = data;
            WorldScene = worldScene;
        }

        public Base WorldScene { get; }
        public List<WorldTile> Data { get; }
    }

    public bool WasGameSaved { get; private set; }
    public int CurrentSubWorldIndex { get; set; } = -1; 

    

    public void SaveGame()
    {
        // Save temporary data into persistent
        MergeTemporaryData();
        SavePersistentWorldData(WorldManager.Instance.WorldData);
        WasGameSaved = true;
    }

    private void MergeTemporaryData()
    {
        // Get all worldscenes/temporary/... files, iterate over them
        Base[] scenes = new Base[10];
        foreach (Base scene in scenes)
        {
            WorldData persistentData = LoadPersistentWorldData(scene);
            TemporaryWorldData temporaryData = LoadTemporaryWorldData(scene);
            temporaryData.Data.ForEach(tile => persistentData.SetTile(tile));
            SavePersistentWorldData(persistentData);
            DestroyTemporaryWorldData(scene);
        }
    }
    
    
    private WorldData LoadPersistentWorldData(Base worldScene)
    {
        string dataPath = worldScene.GetPersistantPath();
        // WorldData loadedData = null;
        //  Load data from file
        return null;
    }

    
    private void SavePersistentWorldData(WorldData data)
    {
        string dataPath = data.WorldScene.GetPersistantPath();
        // Save data into file
    }
    
    private TemporaryWorldData LoadTemporaryWorldData(Base worldScene)
    {
        string dataPath = worldScene.GetTemporaryPath();
        // TemporaryWorldData loadedData = null;
        //  Load data from file
        return null;
    }

    
    public void SaveTemporaryWorldData(TemporaryWorldData data)
    {
        string dataPath = data.WorldScene.GetTemporaryPath();
        // Save data into file
    }

    private void DestroyTemporaryWorldData(Base worldScene)
    {
        string dataPath = worldScene.GetTemporaryPath();
        // Destroy file with data
    }
}
