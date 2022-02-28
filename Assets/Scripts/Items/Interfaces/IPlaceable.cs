using UnityEngine;

public interface IPlaceable : IUsable
{
    public GameObject GetPrefab()
    {
        return null;
    }
}
