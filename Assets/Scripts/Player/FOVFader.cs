using UnityEngine;

public class FOVFader : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Tree")) return;
        Fader fader = other.gameObject.GetComponent<Fader>();
        if (fader is not null)
        {
            fader.FadeOut();
            fader.IsFaded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Tree")) return;
        Fader fader = other.gameObject.GetComponent<Fader>();
        if (fader is not null)
        {
            fader.FadeIn();
            fader.IsFaded = false;
        }
    }
}
