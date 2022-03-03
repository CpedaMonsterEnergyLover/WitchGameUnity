using System;
using UnityEngine;

public class StatBar : MonoBehaviour
{
    public int unitSize;
    public int maxValue;
    public int currentValue;
    public Color unitColor;
    public GameObject unitPrefab;

    private void Start()
    {
        InitBarBackground();
    }

    private void InitBarBackground()
    {
        for (int i = 0; i < Math.Ceiling((float) maxValue / unitSize); i++)
        {
            InstantiateUnit();
        }
    }

    private void SynchWithCurrentValue()
    {
        foreach (Transform child in transform)
        {
            
        }
    }
    
    private void UpdateBarSize(int newSize)
    {
        int differenceInUnits = (newSize - maxValue) / unitSize;
        if (differenceInUnits > 0)
        {
            // Adds new units
            
        }
        else if (differenceInUnits < 0)
        {
            // Removes units from the end
            
        }
    }

    private void InstantiateUnit()
    {
        Instantiate(unitPrefab, transform);
    }
}
