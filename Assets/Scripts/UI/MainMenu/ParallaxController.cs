using System;
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
            panel.SetWidth(leftBound.position.x, rightBound.position.x);
            Vector3 panelPosition = panel.transform.position;
            panelPosition.x = pos.x - (pos.x / panel.Width * xvalue * 1 / panel.speedX);
            panelPosition.y = pos.y - (pos.y / leftBound.position.y * yvalue * 1 / panel.speedY) + panel.offsetY;
            panel.transform.position = panelPosition;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(leftBound.position, 1f);
        Gizmos.DrawSphere(rightBound.position, 1f);
    }
}
