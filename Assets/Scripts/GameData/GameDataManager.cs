using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using WorldScenes;

public class GameDataManager : MonoBehaviour
{

    #region Singleton

    public static GameDataManager Instance;
    
    private void Awake()
    {
        Instance = this;
        FirstGameStart();
    }

    #endregion

    private const string PersDir = "/Save/Persistent/";
    private const string TempDir = "/Save/Temporary/";

    public int CurrentSubWorldIndex { get; set; } = -1;


    private static void FirstGameStart()
    {
        ClearTemp();
    }
    

    public static void SaveAll()
    {
        MergeAllWorldData();
        if(Application.isPlaying) SavePersistentWorldData(WorldManager.Instance.WorldData);
        TileLoader.Instance.temporaryData.Clear(); 
    }


    
    public static void SavePersistentWorldData(WorldData data)
    {
        string dir = Application.persistentDataPath + PersDir;

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(dir + data.WorldScene.FileName, json);
    }

    public static WorldData LoadPersistentWorldData(BaseWorldScene worldScene)
    {
        string path = Application.persistentDataPath + PersDir + worldScene.FileName;
        WorldData loadedData = null;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            loadedData = JsonUtility.FromJson<WorldData>(json);
        }
        
        return loadedData;
    }

    public static void SaveTemporaryWorldData()
    {
        // Save data into file
        string dir = Application.persistentDataPath + TempDir;

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string json = JsonUtility.ToJson(
            new TemporaryWorldData(TileLoader.Instance.temporaryData), false);
        File.WriteAllText(dir + WorldManager.Instance.worldScene.FileName, json);
    }

    public static List<WorldTile> LoadTemporaryWorldData(BaseWorldScene worldScene)
    {
        string path = Application.persistentDataPath + TempDir + worldScene.FileName;
        if (!File.Exists(path)) return null;
        
        string json = File.ReadAllText(path);
        File.Delete(path);
        var loadedData = JsonUtility.FromJson<TemporaryWorldData>(json).Items;
        return loadedData;
    }
    
    private static void MergeAllWorldData()
    {
        // Get all Worldscenes/Temporary/... files, iterate over them
        string dir = Application.persistentDataPath + TempDir;

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
            return;
        }

        DirectoryInfo dirInfo = new DirectoryInfo(dir);
        foreach (FileInfo file in dirInfo.GetFiles())
        {
            string fileName = file.Name;
            string sceneName = fileName.Split('.')[0];
            var scene = WorldScenesCollection.Get(sceneName);
            
            if (scene is null) 
                throw new Exception(
                    $"Undefined worldscene name: \"{sceneName}\".");
            
            var temporaryData = LoadTemporaryWorldData(scene);
            if (temporaryData is null)
                throw new Exception(
                    $"Couldn't load temp data for scene\"{sceneName}\"." +
                    $" File \"{fileName}\" might be corrupted.");
            
            WorldData persistentData = LoadPersistentWorldData(scene);
            if(persistentData is null) 
                throw new Exception(
                    $"Couldn't merge temp data for scene \"{sceneName}\"," +
                    $" because there's no persistent data for this scene.");

            foreach (WorldTile tile in temporaryData)
            {
                persistentData.GetTile(tile.Position.x, tile.Position.y).SetData(tile);
            }
         
            SavePersistentWorldData(persistentData);
        }
    }

    public static void DeleteTemporaryData(BaseWorldScene worldScene)
    {
        string path = Application.persistentDataPath + TempDir + worldScene.FileName;
        if (File.Exists(path)) File.Delete(path);
    }
    
    public static void ClearTemp() => DeleteAllInDir(Application.persistentDataPath + TempDir);
    public static void ClearPers() => DeleteAllInDir(Application.persistentDataPath + PersDir);
    
    private static void DeleteAllInDir(string directory)
    {
        DirectoryInfo dir = new DirectoryInfo(directory);
        foreach (FileInfo file in dir.GetFiles())
        {
            file.Delete();
        }
    }

}
