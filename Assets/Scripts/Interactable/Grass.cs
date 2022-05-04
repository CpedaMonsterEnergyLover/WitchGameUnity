using UnityEngine;

public class Grass : Herb, IInheritsWorldLayerColor
{
    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }
}
