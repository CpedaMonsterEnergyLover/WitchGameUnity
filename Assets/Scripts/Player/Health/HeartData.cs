using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable, CreateAssetMenu(menuName = "Hearts/Heart")]
public class HeartData : ScriptableObject
{
    public HeartOrigin origin;
    public string id;
    public HeartTypeSprite[] heartTypeSprites = {
        new (HeartType.Solid, null),
        new (HeartType.Holed, null),
        new (HeartType.Torned, null),
        new (HeartType.Healed, null),
    };
    public bool ignoreColor;
    public Color color;
    public Color outlineColor;
    public List<DamageType> resistDamageTypes = new();
    public List<DamageType> immuneDamageTypes = new();
}
