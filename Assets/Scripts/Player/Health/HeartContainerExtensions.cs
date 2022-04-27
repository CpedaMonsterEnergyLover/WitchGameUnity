﻿using System.Collections.Generic;
using UnityEngine;

public partial class HeartContainer
{
    
    // ----------------------Finding methods------------------------
    public List<int> FindAllByType(HeartType targetType)
    {
        var indexes = new List<int>();
        for (int i = 0; i < _addedCount; i++)
            if (_hearts[i].Type == targetType)
                indexes.Add(i);
        return indexes;
    }
    
    public List<int> FindAllByOrigin(HeartOrigin targetOrigin)
    {
        var indexes = new List<int>();
        for (int i = 0; i < _addedCount; i++)
            if (_hearts[i].Data.origin == targetOrigin)
                indexes.Add(i);
        return indexes;
    }
    
    
    public List<int> FindRandomByType(HeartType targetType, int amount)
    {
        var indexes = FindAllByType(targetType);
        int count = indexes.Count;
        if (amount >= count) return indexes;
        amount = count - amount;
        for (int i = 0; i < amount; i++) 
            indexes.RemoveAt(Random.Range(0, indexes.Count));
        return indexes;
    }
    
    public List<int> FindRandomByOrigin(HeartOrigin targetOrigin, int amount)
    {
        var indexes = FindAllByOrigin(targetOrigin);
        int count = indexes.Count;
        if (amount >= count) return indexes;
        amount = count - amount;
        for (int i = 0; i < amount; i++) 
            indexes.RemoveAt(Random.Range(0, indexes.Count));
        return indexes;
    }

    public List<int> FindAllWithEffect()
    {
        return null;
    }
    
    public List<int> FindRandomWithEffect(int amount)
    {
        return null;
    }
    
    
    
    // ----------------------Editing methods------------------------
    public void PopAll(List<int> indexes)
    {
        foreach (int i in indexes) Pop(i);
    }

    public void ChangeOriginAll(List<int> indexes, HeartOrigin newOrigin)
    {
        foreach (int i in indexes) ChangeOrigin(i, newOrigin);
    }

    public void HealAll(List<int> indexes)
    {
        foreach (int i in indexes) _hearts[i].Heal();
    }

    public void ApplyEffectAll(List<int> indexes)
    {
        foreach(int i in indexes) _hearts[i].ApplyEffect();
    }
    
    public void RemoveEffectAll(List<int> indexes)
    {
        foreach(int i in indexes) _hearts[i].RemoveEffect();
    }
    
}