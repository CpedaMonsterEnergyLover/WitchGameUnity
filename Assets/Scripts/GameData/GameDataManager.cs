using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using WorldScenes;

public class GameDataManager : MonoBehaviour
{
    private void Awake()
    {
        InitDirPaths();
        ClearTemp();
    }

    public static void InitDirPaths()
    {
        _tempDir = Application.persistentDataPath + TempDir;
        _persDir = Application.persistentDataPath + PersDir;
        _playerDir = Application.persistentDataPath + PlayerDir;
    }

    private const string PersDir = "/Save/Persistent/";
    private const string TempDir = "/Save/Temporary/";
    private const string PlayerDir = "/Save/Player/";
    private static string _tempDir;
    private static string _persDir;
    private static string _playerDir;

    public int CurrentSubWorldIndex { get; set; } = -1;
    

    public static async UniTask SaveAll()
    {
        await MergeAllWorldData();
        WorldManager.Instance.UnloadAllEntities();
        Debug.Log("Unloaded entities");
        TileLoader.Instance.Reload();
        Debug.Log("Reloaded tile loader");
        SavePlayerData();
        await SavePersistentWorldData(WorldManager.Instance.WorldData);
    }

    public static bool HasSavedOverWorld()
    {
        string dir = _persDir;
        string path = dir + "OverWorld.json";
        return Directory.Exists(dir) && File.Exists(path);
    }
    
    
    public static async UniTask SavePersistentWorldData(WorldData data)
    {
        string dir = _persDir;

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string json = JsonUtility.ToJson(data, true);
        await File.WriteAllTextAsync(dir + data.WorldScene.FileName, json);
    }

    private static void SavePlayerData()
    {
        string dir = _playerDir;

        if (!Directory.Exists(_playerDir))
            Directory.CreateDirectory(_playerDir);

        string json = JsonUtility.ToJson(PlayerData.Build(), true);
        File.WriteAllText(dir + "PlayerData.json", json);
    }

    public static PlayerData LoadPlayerData()
    {
        string path = _playerDir + "PlayerData.json";
        PlayerData loadedData = null;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            loadedData = JsonUtility.FromJson<PlayerData>(json);
        }
        
        return loadedData;
    }
    
    public static WorldData LoadPersistentWorldData(BaseWorldScene worldScene)
    {
        string path = _persDir + worldScene.FileName;
        WorldData loadedData = null;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            loadedData = JsonUtility.FromJson<WorldData>(json);
            loadedData.Init();
        }
        
        return loadedData;
    }

    public static void SaveTemporaryWorldData()
    {
        string dir = _tempDir;

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        WorldManager.Instance.UnloadAllEntities();
        // TODO: save changed tiles in an array
        var changedTiles = 
            WorldManager.Instance.WorldData.Changes;
        string json = JsonUtility.ToJson(new TemporaryWorldData(changedTiles), true);
        File.WriteAllText(dir + WorldManager.Instance.worldScene.FileName, json);
        
    }

    public static List<WorldTile> LoadTemporaryWorldData(BaseWorldScene worldScene)
    {
        string path = _tempDir + worldScene.FileName;
        if (!File.Exists(path)) return null;
        
        string json = File.ReadAllText(path);
        File.Delete(path);
        var loadedData = JsonUtility.FromJson<TemporaryWorldData>(json).Items;
        return loadedData;
    }
    
    private static async UniTask MergeAllWorldData()
    {
        Debug.Log("Start merge");
        string dir = _tempDir;

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
            
            if(scene == WorldManager.Instance.worldScene) return;

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
                persistentData.GetTile(tile.Position.x, tile.Position.y)
                    .MergeData(tile, false);
            }
            
            Debug.Log($"Merging {fileName}");
         
            await SavePersistentWorldData(persistentData);
        }
        Debug.Log("Merge complete");

        await UniTask.Yield();
    }

    public static void DeleteTemporaryData(BaseWorldScene worldScene)
    {
        string path = _tempDir + worldScene.FileName;
        if (File.Exists(path)) File.Delete(path);
    }

    private static void ClearTemp() => DeleteAllInDir(_tempDir);
    private static void ClearPers() => DeleteAllInDir(_persDir);
    private static void ClearPlayerData() => DeleteAllInDir(_playerDir);

    public static void ClearAllData()
    {
        ClearPers();
        ClearTemp();
        ClearPlayerData();
    }
    
    private static void DeleteAllInDir(string directory)
    {
        DirectoryInfo dir = new DirectoryInfo(directory);
        foreach (FileInfo file in dir.GetFiles())
        {
            file.Delete();
        }
    }

}
