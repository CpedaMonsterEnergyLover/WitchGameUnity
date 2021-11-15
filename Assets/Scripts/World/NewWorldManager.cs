using UnityEngine;
using UnityEngine.Tilemaps;

public class NewWorldManager : MonoBehaviour
{
    #region Vars

    // Public
    public Tilemap GroundTilemap;
    public TileBase pathTiledata;
    public TileBase grassTiledata;
    public int mapSize;
    
    // Private 

    #endregion



    #region UnityMethods

    private void Start()
    {
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                GroundTilemap.SetTile(new Vector3Int(x, y, 0), 
                    Random.Range(1, 4) <= 2 ? pathTiledata : grassTiledata);

            }
        }
    }

    #endregion



    #region ClassMethods



    #endregion
}
