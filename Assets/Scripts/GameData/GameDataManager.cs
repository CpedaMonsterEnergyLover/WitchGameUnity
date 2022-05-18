using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    [SerializeField] private GameCollection.Manager gameCollectionManager;

    private static bool _firstBoot = true;
    public static PlayerData PlayerData { get; private set; }
    
    private void Awake()
    {
        if (!_firstBoot) return;
        InitDirPaths();
        ClearTemp();
        gameCollectionManager.Init();
        PlayerData = LoadPlayerData();
        WorldPositionProvider.PlayerPosition = PlayerData?.Position ?? Vector2.negativeInfinity;
        _firstBoot = false;
    }

    private const string PersDir = "/Save/Persistent/";
    private const string TempDir = "/Save/Temporary/";
    private const string PlayerDir = "/Save/Player/";
    private static string _tempDir;
    private static string _persDir;
    private static string _playerDir;
    
    public static void InitDirPaths()
    {
        _tempDir = Application.persistentDataPath + TempDir;
        _persDir = Application.persistentDataPath + PersDir;
        _playerDir = Application.persistentDataPath + PlayerDir;
    }
    
    public static async UniTask SaveAll()
    {
        await MergeAllWorldData();
        WorldManager.Instance.UnloadAllEntities();
        Debug.Log("Unload entities");
        TileLoader.Instance.Reload();
        Debug.Log("Reload tile loader");
        SavePlayerData();
        await SavePersistentWorldData(WorldManager.Instance.WorldData);
    }

    public static bool HasSavedOverWorld()
    {
        string dir = _persDir;
        string path = dir + "OverWorld.json";
        return Directory.Exists(dir) && File.Exists(path);
    }
    
    
    public static async UniTask SavePersistentWorldData(WorldData data, string overrideFileName = null)
    {
        string dir = _persDir;
        
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        StringBuilder sb = new StringBuilder()
            .Append(dir);
        sb.Append(overrideFileName ?? data.WorldScene.GetFileName());

        string json = JsonUtility.ToJson(data, true);
        await File.WriteAllTextAsync(sb.ToString(), json);
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
        CountWorldParts();
        
        return loadedData;
    }
    
    public static WorldData LoadPersistentWorldData(string fileName)
    {
        string path = _persDir + fileName;
        WorldData loadedData = null;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            loadedData = JsonUtility.FromJson<WorldData>(json);
            loadedData.Init();
        }
        
        return loadedData;
    }

    private static void CountWorldParts()
    {
        Dictionary<string, int> sceneCounts = new();
        
        DirectoryInfo directoryInfo = new DirectoryInfo(_persDir);
        FileInfo[] files = directoryInfo.GetFiles("*.json");
        foreach (FileInfo info in files)
        {
            string sceneName = info.Name.Split("_")[0];
            if (!WorldScenesCollection.MultipartSceneNames.Contains(sceneName)) continue;
            if (sceneCounts.ContainsKey(sceneName)) sceneCounts[sceneName]++;
            else sceneCounts.Add(sceneName, 1);
        }

        foreach (var (key, value) in sceneCounts)
            ((MultipartWorldScene) WorldScenesCollection.Get(key)).SubWorldsCount = value;
    }
    
    public static async UniTask SaveTemporaryWorldData()
    {
        string dir = _tempDir;

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        WorldManager.Instance.UnloadAllEntities();
        
        // TODO: rework changes gathering algorithm
        List<WorldTile> changedTiles = WorldManager.Instance.WorldData.Changes;
        string json = JsonUtility.ToJson(new TemporaryWorldData(changedTiles), true);
        await File.WriteAllTextAsync(dir + WorldManager.Instance.worldScene.GetFileName(), json);
    }

    public static List<WorldTile> LoadTemporaryWorldData(string fileName)
    {
        string path = _tempDir + fileName;
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
            string sceneName = fileName.Split('.')[0].Split("_")[0];
            var scene = WorldScenesCollection.Get(sceneName);
            
            
            if (scene is null) 
                throw new Exception(
                    $"Undefined worldscene name: \"{sceneName}\".");
            
            if(sceneName == WorldManager.Instance.worldScene.sceneName) return;

            var temporaryData = LoadTemporaryWorldData(fileName);
            if (temporaryData is null)
                throw new Exception(
                    $"Couldn't load temp data for scene\"{sceneName}\"." +
                    $" File \"{fileName}\" might be corrupted.");
            
            WorldData persistentData = LoadPersistentWorldData(fileName);
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
         
            await SavePersistentWorldData(persistentData, fileName);
        }
        Debug.Log("Merge complete");

        await UniTask.Yield();
    }

    public static void DeleteTemporaryData(WorldScene worldScene)
    {
        string path = _tempDir + worldScene.GetFileName();
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
        if(!Directory.Exists(directory)) return;
        DirectoryInfo dir = new DirectoryInfo(directory);
        foreach (FileInfo file in dir.GetFiles())
        {
            file.Delete();
        }
    }

}
