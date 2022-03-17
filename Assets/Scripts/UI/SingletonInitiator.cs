using System.Collections.Generic;
using UnityEngine;

public class SingletonInitiator : MonoBehaviour
{
    public List<GameObject> objectsToEnable;

    private void Awake()
    {
        objectsToEnable.ForEach(o =>
        {
            bool wasActive = o.activeInHierarchy;
            o.SetActive(true);
            o.SetActive(wasActive);
        } );
    }
}
