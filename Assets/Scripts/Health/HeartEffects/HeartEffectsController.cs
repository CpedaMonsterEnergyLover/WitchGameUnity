using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartEffectsController : MonoBehaviour
{
    [SerializeField] private HeartContainer container;
    private readonly Dictionary<HeartEffect, int> _effectDurations = new();
    private readonly Dictionary<HeartEffect, Coroutine> _effectRoutines = new ();

    public void StartEffect(Heart heart, HeartEffect effect)
    {
        Coroutine effectRoutine = StartCoroutine(EffectRoutine(heart, effect));
        _effectRoutines.Add(effect, effectRoutine);
    }

    public void StopEffect(int index, HeartEffect effect)
    {
        _effectDurations.Remove(effect);
        _effectRoutines.Remove(effect);
        container.ClearEffect(index);
    }

    public void AddTime(int index, HeartEffect from, int time)
    {
        int currentDuration = _effectDurations[from];
        currentDuration += time;
        if (currentDuration <= 0)
        {
            StopEffect(index, from);
            StopCoroutine(_effectRoutines[from]);
            from.Tick(container, index);
        }
        else
        {
            _effectDurations[from] = currentDuration;
            container.UpdateEffectTime(index, currentDuration);
        }
        
    }

    private IEnumerator EffectRoutine(Heart heart, HeartEffect effect)
    {
        _effectDurations.Add(effect, effect.Duration);
        while (_effectDurations[effect] > 0)
        {
            _effectDurations[effect]--;
            container.UpdateEffectTime(heart.Index, _effectDurations[effect]);
            yield return new WaitForSeconds(1f);
        }
        StopEffect(heart.Index, effect);
        effect.Tick(container, heart.Index);
    }
}
