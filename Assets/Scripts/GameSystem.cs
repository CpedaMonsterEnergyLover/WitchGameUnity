using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public GameCollection.Manager collectionManager;
    public List<GameObject> toDisableWhileLoading = new();
    
    public static GameSystem Instance { get; private set; }

    private void Start()
    {
        if (Instance is null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DisableObjects();
            Time.timeScale = 0;
            void AfterLoading()
            {
                EnableObjects();
                Time.timeScale = 1;
            }
            WorldManager.ONWorldLoaded += AfterLoading;
        }
        else
        {
            GameObject o;
            (o = gameObject).SetActive(false);
            DestroyImmediate(o);
        }
    }

    private void DisableObjects()
    {
        foreach (GameObject o in toDisableWhileLoading) o.SetActive(false);
    }

    private void EnableObjects()
    {
        foreach (GameObject o in toDisableWhileLoading) o.SetActive(true);
    }
    
    // Generator -> manager -> collection -> gamesystem
}
