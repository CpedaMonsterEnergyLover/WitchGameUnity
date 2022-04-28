using System;
using UnityEngine;

[Serializable]
public abstract class Heart
{
    [SerializeField] protected HeartData heartData;
    [SerializeField] protected HeartType heartType;
    [SerializeField] protected HeartEffect heartEffect;

    public bool HasEffect => heartEffect is not null;
    public HeartUnit Unit { get; private set; }
    public HeartData Data => heartData;
    public HeartType Type => heartType;

    public int Index { get; set; }

    
    protected Heart(HeartData data, HeartType type)
    {
        heartData = data;
        heartType = type;
    }

    public void SetUnit(HeartUnit unit)
    {
        unit.HeartData = heartData;
        unit.HeartType = heartType;
        Unit = unit;
    }


    public void ApplyEffect(HeartEffect toApply, HeartEffectsController controller)
    {
        HeartEffectType appliableType = toApply.EffectData.effectType;
        if(heartData.immuneEffects.Contains(appliableType)) return;
        if (heartEffect is null)
        {
            heartEffect = toApply;
            Unit.StartEffect(toApply);
            controller.StartEffect(this, toApply);
        }
        else if (heartEffect.EffectData.effectType == appliableType)
        {
            heartEffect.Stack();
        }
        else
        {
            // Applying effect is different from current
        }
    }
    

    public void ClearEffect()
    {
        heartEffect = null;
        Unit.ClearEffect();
    }
    
    public void RemoveEffect() { }
    
    public bool ApplyDamage(DamageType damageType, BulletSize bulletSize)
    {
        if (heartType is HeartType.Holed &&
            bulletSize is BulletSize.Small ||
            heartData.immuneDamageTypes.Contains(damageType))
        {
            Unit.PlayImmune();
            return false;
        }
        if (heartType is HeartType.Solid or HeartType.Healed && 
            heartData.resistDamageTypes.Contains(damageType))
        {
            heartType = HeartType.Torned;
            Unit.HeartType = HeartType.Torned;
            Unit.PlayDamaged();
            OnDamageTake();
            return false;
        }
        return true;
    }

    public void Heal()
    {
        if(heartType != HeartType.Torned) return;
        heartType = HeartType.Healed;
        Unit.HeartType = HeartType.Healed;
        Unit.PlayHeal();
    }

    public virtual void OnCreated() { }
    
    protected virtual void OnDamageTake() { }

    public virtual void Pop()
    {
        
    }
    
    
    
    public override string ToString()
    {
        return $"{GetType()}:{heartData.id}";
    }

    public static Heart Create(HeartOrigin origin, HeartType type)
    {
        return origin switch
        {
            HeartOrigin.Human => new HumanHeart(type),
            HeartOrigin.Shadow => new ShadowHeart(type),
            HeartOrigin.Beast => new HeartOfTheBeast(type),
            HeartOrigin.Demonic => new DemonicHeart(type),
            HeartOrigin.Spectral => new SpectralHeart(type),
            HeartOrigin.Wild => new HeartOfTheWild(type),
            HeartOrigin.Archdemonic => new ArchDemonicHeart(type),
            _ => throw new ArgumentOutOfRangeException(nameof(origin), origin, null)
        };
    }
}
