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
    
    private void Start()
    {
        WorldData world = WorldManager.Instance.WorldData;
        float w = world.MapWidth;
        float h = world.MapHeight;
        left.size = new Vector2(x, h);
        top.size = new Vector2(w, y);
        right.size = new Vector2(x, h);
        bottom.size = new Vector2(w, y);
        left.offset = new Vector2(x / 2, h / 2);
        top.offset = new Vector2(w / 2, h - y / 2);
        right.offset = new Vector2(w - x / 2, h / 2);
        bottom.offset = new Vector2(w / 2, y / 2);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        DialogWindow.Instance.StartDialog(dialogTree);
    }
}
