using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldManager : MonoBehaviour
{
    #region Vars

    public Generator generator;
    public Transform playerTransform;

    #endregion



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
