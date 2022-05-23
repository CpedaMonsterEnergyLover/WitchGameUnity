using System;
using UnityEngine;

public class WorldBounds : MonoBehaviour
{
    public DialogTree dialogTree;
    public BoxCollider2D left;
    public BoxCollider2D top;
    public BoxCollider2D right;
    public BoxCollider2D bottom;
    public float x;
    public float y;

    private void Awake()
    {
        WorldManager.ONWorldLoaded += ActivateBounds;
    }

    private void OnDestroy()
    {
        WorldManager.ONWorldLoaded -= ActivateBounds;
    }

    private void ActivateBounds()
    {
        WorldData worldData = WorldManager.Instance.WorldData;
        float w = worldData.MapWidth;
        float h = worldData.MapHeight;
        left.size = new Vector2(x, h);
        top.size = new Vector2(w, y);
        right.size = new Vector2(x, h);
        bottom.size = new Vector2(w, y);
        left.offset = new Vector2(x / 2, h / 2);
        top.offset = new Vector2(w / 2, h - y / 2);
        right.offset = new Vector2(w - x / 2, h / 2);
        bottom.offset = new Vector2(w / 2, y / 2);
    }

    // No need to check if other collider is player because worldBorder layer is only collides with player layer
    private void OnCollisionEnter2D(Collision2D other)
    {
        DialogWindow.Instance.StartDialog(dialogTree);
    }
}
