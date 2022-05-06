using System;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public GameCollection.Manager collectionManager;
    public List<GameObject> toDisableWhileLoading = new();
    
    public static GameSystem Instance { get; private set; }

    private void Start()
    {
        if (Instance != this)
        {
            Instance = this;
            DisableObjects();
            Time.timeScale = 0;
        
            
    
            WorldManager.ONWorldLoaded += AfterLoading;
        }
        else
        {
            GameObject o;
            (o = gameObject).SetActive(false);
            Destroy(o);
        }
        
        
    }

    private void OnDestroy()
    {
        WorldManager.ONWorldLoaded -= AfterLoading;
    }

    private void AfterLoading()
    {
        EnableObjects();
        Time.timeScale = 1;
    }

    private void DisableObjects()
    {
        foreach (GameObject o in toDisableWhileLoading) o.SetActive(false);
    }

    private void EnableObjects()
    {
        foreach (GameObject o in toDisableWhileLoading)
            o.SetActive(true);
    }
    
}
