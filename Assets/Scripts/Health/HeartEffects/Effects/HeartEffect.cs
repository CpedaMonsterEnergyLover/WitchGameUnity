using System;
using UnityEngine;

[Serializable]
public abstract class HeartEffect
{
    [SerializeField] protected HeartEffectData effectData;
    [SerializeField] protected int duration;
    
    public int Duration => duration;
    public HeartEffectData EffectData => effectData;
    
    protected HeartEffect(HeartEffectData data)
    {
        effectData = data;
        duration = data.startingDuration;
    }


    
    public abstract void Stack();
    public abstract void Tick(HeartContainer container, int index);

    public static HeartEffect Create(HeartEffectData data)
    {
        return data.effectType switch
        {
            HeartEffectType.Burn => new BurnEffect(data),
            HeartEffectType.Poison => new PoisonEffect(data),
            HeartEffectType.Curse => new CurseEffect(data),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    
}
