using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Object = UnityEngine.Object;

public class Chunk
{
    #region Vars

    // Public
    public GameObject gameObject;
    public Vector2 cords;

    // Private

    #endregion



    #region ClassMethods

    // Конструктор
    public Chunk(GameObject prefab, Transform parent, int chunkSize, Vector2 position)
    {
        cords = position;
        
        gameObject = Object.Instantiate(prefab, parent);
        gameObject.transform.localScale = new Vector3(chunkSize, chunkSize, 1);
        gameObject.transform.position = new Vector3(
            position.x * chunkSize + chunkSize / 2f, 
            position.y * chunkSize + chunkSize / 2f, 
            0);
        gameObject.transform.rotation = Quaternion.AngleAxis(180f, new Vector3(0, 1f, 0));

        gameObject.name = $"chunk({position.x}:{position.y})";
    }

    public bool TooFarFromPlayer(Vector2 playerPosition, int viewRangeX, int viewRangeY)
    {
        // если слишком далеко от игрока - удаляет чанк
        if (!InDistance(cords,playerPosition, viewRangeX, viewRangeY))
        {
            Object.Destroy(gameObject);
            return true;
        }

        return false;
    }

    #endregion



    #region UtilMethods

    // Возвращает координаты чанка по мировым координатам
    public static Vector2 Of(float x, float y, int chunkSize)
    {
        Vector2 chunkCords = new Vector2(
            (int) x / chunkSize - (x < 0 ? 1 : 0),
            (int) y / chunkSize - (y < 0 ? 1 : 0));
        return chunkCords;
    }

    public static bool InDistance(Vector2 from, Vector2 to, int distanceX, int distanceY)
    {
        float distX = Math.Abs(from.x - to.x);
        float distY = Math.Abs(from.y - to.y);
        return distX < distanceX && distY < distanceY;
    }
    
    #endregion
}
