using UnityEngine;

public static class VectorUtil 
{
    public static Vector2 RotateVector2ByDegree(Vector2 toRotate, float degree)
    {
        return Quaternion.Euler(0, 0, degree) * toRotate;
    } 
    
    public static Vector2 RandomOnCircle(float radius = 1f)
    {
        return Random.insideUnitCircle.normalized * radius;
    }
}
