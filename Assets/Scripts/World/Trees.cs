using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class Trees : ScriptableObject
{
    [SerializeField]
    public List<RuleTile> collection;
}

