using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
public class ParallaxPanel : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    [FormerlySerializedAs("speed")] public float speedX;
    public float offsetY;
    public float speedY;

    
    public float Width { get; private set; }

    public void SetWidth(float leftBound, float rightBound)
    {
        Width = (leftBound - rightBound) * -1;
        spriteRenderer.size =
            new Vector2(Width, spriteRenderer.size.y);
    }
}
