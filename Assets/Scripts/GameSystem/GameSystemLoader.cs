using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameSystemLoader : MonoBehaviour
{
    [SerializeField] private GameObject gameSystemPrefab;
    [SerializeField] private bool createOnStart;
    [SerializeField] private List<GameObject> toDisableWhileInstantiating;


    public void CreateGameSystem()
    {
        if(Application.isEditor && GameObject.FindWithTag("GameSystem") is not null) return;
        if(Application.isPlaying && GameSystem.Instance is not null) return;
        
        PrettyDebug.Log("Instantiating", this);
        GameObject gameSystemGO;
        if (Application.isPlaying) gameSystemGO = Instantiate(gameSystemPrefab);
        else gameSystemGO = (GameObject) PrefabUtility.InstantiatePrefab(gameSystemPrefab);
        gameSystemGO.transform.SetAsLastSibling();
    }

    public void DestroyGameSystem()
    {
        GameObject gameSystemGO = GameObject.FindWithTag("GameSystem");
        if(gameSystemGO is null) return;
        PrettyDebug.Log("Destroying", this);
        if(Application.isPlaying) Destroy(gameSystemGO);
        else DestroyImmediate(gameSystemGO);
    }

    private void Awake()
    {
        PrettyDebug.Log("Awake", this);
        if (!createOnStart)
        {
            PrettyDebug.Log("Not creating on awake", this);
            return;
        }
        if(GameSystem.Instance is not null) return;
        DisableObjects();
        GameSystem.AwakeCallback = Finish;
        CreateGameSystem();
    }
    
    private void DisableObjects()
    {
        foreach (GameObject o in toDisableWhileInstantiating) 
            o.SetActive(false);
    }

    private void Finish()
    {
        foreach (GameObject o in toDisableWhileInstantiating)
            o.SetActive(true);
    }
}
