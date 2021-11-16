using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldManager : MonoBehaviour
{
    #region Vars

    public Generator generator;
    public Transform playerTransform;

    #endregion

    /// <summary>
    /// Песок /// пока не нада
    /// Каменистая земля
    /// Трава (полевая)
    /// зеленая трава (лесная)
    /// Вода
    ///
    /// Полевая должна сочетаться с лесной
    ///
    /// Вода это отдельный слой
    /// Каменистая земля это отдельный слой
    /// 
    /// </summary>

    #region UnityMethods

    private void Start()
    {
        if(generator.GenerateOnStart) Generate();
    }

    #endregion



    #region ClassMethods

    public void Generate()
    {
        generator.ClearAllTiles();
        generator.GenerateWorld();
    }

    #endregion
}
