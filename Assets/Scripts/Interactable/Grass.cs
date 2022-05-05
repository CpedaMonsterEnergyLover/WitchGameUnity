using UnityEngine;

public class Grass : Herb, IColorableInteractable
{
    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }
}
