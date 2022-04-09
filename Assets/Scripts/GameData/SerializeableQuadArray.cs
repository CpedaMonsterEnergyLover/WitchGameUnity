using System.Collections;
using UnityEngine;

[System.Serializable]
public class SerializeableQuadArray<T> : IEnumerable
{
    [SerializeField]
    private T[] items;
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;

    public T Get(int x, int y) => items[x + y * height];
    public void Set(int x, int y, T item) => items[x + y * height] = item;
    public int Width => width;
    public int Height => height;

    public SerializeableQuadArray(int width, int height)
    {
        items = new T[width * height];
        this.width = width;
        this.height = height;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable) this).GetEnumerator();
    }
}
