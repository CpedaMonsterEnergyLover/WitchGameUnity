using UnityEngine;

public class HeartAnimationHelper : MonoBehaviour
{
    public void UpdateHeart()
    {
        transform.parent.GetComponent<Heart>().UpdateInstant();
    }

    public void RemoveHeart()
    {
        Heart heart = transform.parent.GetComponent<Heart>();
        heart.gameObject.SetActive(false);
        heart.isPopping = false;
    }
}
