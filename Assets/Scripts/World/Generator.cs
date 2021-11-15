using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generator : MonoBehaviour
{
    #region Vars

    // Public
    public int worldWidth;
    public Vector2Int cardinalPoints;
    public float[] cardinalMap;
    
    // Private

    #endregion



    #region UnityMethods

    private void Start()
    {
        cardinalPoints = GetCardinalPoints();
    }

    #endregion



    #region ClassMethods


    private void GenerateCardinalMap()
    {
        
    }
    
    private Vector2Int GetCardinalPoints()
    {
        int startingPoint = Random.Range(0, 2) * 2 - 1;
        return new Vector2Int(startingPoint, startingPoint * -1);
    }

    #endregion
}
