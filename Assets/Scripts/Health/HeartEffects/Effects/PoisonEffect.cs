using UnityEngine;

public class PoisonEffect : HeartEffect
{
    public PoisonEffect(HeartEffectData data) : base(data)
    {
    }

    public override void Stack()
    {
        
    }

    public override void Tick(HeartContainer container, int index)
    {
        container.Pop(index);
        if(container.IsEmpty) return;
        
        var indexes = container.FindAllWithNoEffects();
        if(indexes.Count == 0) return;
        
        int random = Random.Range(0, indexes.Count);
        container.ApplyEffect(indexes[random], GameCollection.Hearts.GetEffect("poison"));
    }
}
