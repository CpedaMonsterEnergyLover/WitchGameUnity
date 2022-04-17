using System;
using UnityEngine;

[Serializable]
public struct BossStage
{
    public int health;
    public Color healthColor;
    public bool hasDialog;
    public DialogTree startingDialog;
}
