using System.Collections;
using UnityEngine;

// TODO: Rework
public class Fader : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer { get; private set; }
    private Color _color;
    private Coroutine _routine;
    public bool IsFaded { get; set; }
    public bool IsBlocked { get; set; }
    public float Speed { get; set; } = 0.05f;
    

    private void Start()
    {
        SpriteRenderer = GetComponentInParent<SpriteRenderer>();
        _color = SpriteRenderer.color;
    }

    public void FadeOut(float amount)
    {
        if (IsBlocked) return;
        if(_routine is not null) StopCoroutine(_routine);
        if(!gameObject.activeInHierarchy) return;
        _routine = StartCoroutine(Fade(_color.a, amount, - Speed));
    }

    public void FadeIn()
    {
        if(_routine is not null) StopCoroutine(_routine);
        if(!gameObject.activeInHierarchy) return;
        _routine = StartCoroutine(Fade(_color.a, 1f, Speed));
    }

    private IEnumerator Fade(float from, float to, float direction)
    {
        if (direction < 0)
        {
            for (float ft = from;  ft > to; ft += direction)
            {
                _color.a = ft;
                SpriteRenderer.color = _color;
                yield return new WaitForSeconds(.01f);
            }
        }
        else
        {
            for (float ft = from;  ft < to; ft += direction)
            {
                _color.a = ft;
                SpriteRenderer.color = _color;
                yield return new WaitForSeconds(.01f);
            }
        }
        
    }
}
