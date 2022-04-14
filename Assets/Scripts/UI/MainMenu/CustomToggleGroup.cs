using System.Collections.Generic;
using UnityEngine;

public abstract class CustomToggleGroup<T> : MonoBehaviour
{
    public List<T> values;
    public T value;

    public void Select(int index) => value = values[index];
}
