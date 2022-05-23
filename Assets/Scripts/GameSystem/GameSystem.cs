using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    [SerializeField] private List<GameObject> toDisableWhileLoading = new();
    
    public static GameSystem Instance { get; private set; }
    public static Action AwakeCallback { get; set; }

    private static bool _callbackPlayed;    
    
    private void Awake()
    {
        if(!_callbackPlayed) AwakeCallback?.Invoke();
        _callbackPlayed = true;
        DontDestroyOnLoad(this);
        Instance = this;
        DisableObjects();
        Time.timeScale = 0;
        WorldManager.ONWorldLoaded += AfterLoading;
    }

    private void OnDestroy()
    {
        Instance = null;
        _callbackPlayed = false;
        WorldManager.ONWorldLoaded -= AfterLoading;
    }

    private void AfterLoading()
    {
        EnableObjects();
        Time.timeScale = 1;
    }

    private void DisableObjects()
    {
        foreach (GameObject o in toDisableWhileLoading) 
            o.SetActive(false);
    }

    private void EnableObjects()
    {
        foreach (GameObject o in toDisableWhileLoading)
            o.SetActive(true);
    }
    
}
