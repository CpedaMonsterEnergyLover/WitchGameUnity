using UnityEngine;

public class CaveBorder : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;

    private void Awake()
    {
        WorldManager.ONWorldLoaded += UpdateBorder;
    }

    private void OnDestroy()
    {
        WorldManager.ONWorldLoaded -= UpdateBorder;
    }

    private void UpdateBorder()
    {
        WorldData data = WorldManager.Instance.WorldData;
        int w = data.MapWidth;
        int h = data.MapHeight;
        rectTransform.sizeDelta = new Vector2(w, h);
        rectTransform.position = new Vector3(w * 0.5f, h * 0.5f, 0);
    }
}