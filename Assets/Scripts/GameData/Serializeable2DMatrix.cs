using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Serializeable2DMatrix<T> : IEnumerable 
{
    [SerializeField]
    private T[] items;
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    
    public IEnumerable<T> Items => items;

    public bool IsValid(int x, int y) => x >= 0 && x < width && y >= 0 && y < height;
    
    public T Get(int x, int y) => IsValid(x, y) ? items[GetIndex(x, y)] : default;
    
    public void Set(int x, int y, T item)
    {
        if (IsValid(x, y)) items[GetIndex(x, y)] = item;
    }
    public int Width => width;
    public int Height => height;

    public Serializeable2DMatrix(int width, int height)
    {
        items = new T[width * height];
        this.width = width;
        this.height = height;
    }

    private int GetIndex(int x, int y)
    {
        return x + y * width;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable) this).GetEnumerator();
    }

}
