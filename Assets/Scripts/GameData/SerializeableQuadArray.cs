using System.Collections;
using System.Collections.Generic;
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

    public IEnumerable<T> Items => items;
    
    public bool IsValid(int x, int y) => x >= 0 && x < width && y >= 0 && y < height;
    
    public T Get(int x, int y) => 
        !IsValid(x, y) ? default : items[x + y * height];
    
    public void Set(int x, int y, T item)
    {
        if (!IsValid(x, y))
        {
            Debug.Log($"{x}, {y} is not valid");
            return;
        }
        items[x + y * height] = item;
    }

    public static void Test(int abc)
    {
        return;
    }

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
