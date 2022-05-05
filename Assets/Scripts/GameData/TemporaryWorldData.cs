using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TemporaryWorldData : IEnumerable
{
    [SerializeField] private List<WorldTile> items;

    public List<WorldTile> Items => items;

    public TemporaryWorldData(List<WorldTile> items)
    {
        this.items = items;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable) this).GetEnumerator();
    }
}