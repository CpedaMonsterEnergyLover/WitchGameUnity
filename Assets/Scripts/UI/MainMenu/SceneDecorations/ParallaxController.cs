using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParallaxController : MonoBehaviour
{
    public Transform leftBound;
    public Transform rightBound;
    public List<ParallaxPanel> panels = new();
    [Range(0, 100)]
    public int xvalue;
    [Range(0, 100)]
    public int yvalue;

    
    private void Update()
    {
        Vector3 pos = transform.position;
        foreach (ParallaxPanel panel in panels)
        {
            var position = leftBound.position;
            panel.SetWidth(position.x, rightBound.position.x);
            var transform1 = panel.transform;
            Vector3 panelPosition = transform1.position;
            panelPosition.x = pos.x - (pos.x / panel.Width * xvalue * 1 / panel.speedX);
            panelPosition.y = pos.y - (pos.y / position.y * yvalue * 1 / panel.speedY) + panel.offsetY;
            transform1.position = panelPosition;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(leftBound.position, 1f);
        Gizmos.DrawSphere(rightBound.position, 1f);
    }
}
