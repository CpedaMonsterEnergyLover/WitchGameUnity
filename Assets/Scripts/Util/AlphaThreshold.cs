using UnityEngine;
using UnityEngine.UI; 

// Нужно для того, чтобы проверка курсора над изображением срабатывала
// Даже на его прозрачных частях
[RequireComponent(typeof(Image))]
public class AlphaThreshold : MonoBehaviour
{
    [Range(0, 1)]
    public float threshold;
    private void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = threshold;
    }
}