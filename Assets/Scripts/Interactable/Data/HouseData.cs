using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Interactable/House")]
public class HouseData : InteractableData, IWorldTransitionData
{
    [Header("HouseData")] 
    public MultipartWorldScene worldScene;
    [Range(3, 20)]
    public int roomWidth;
    [Range(3, 20)]
    public int roomHeight;
    [Range(2, 19)]
    public int doorPosition;

    private int ActualWidth => roomWidth + 3;
    private int ActualHeight => roomHeight + 4;
    
    public WorldData GetData()
    {
        // Look for save
        // If no save, generate 
        return GenerateData();
    }

    private WorldData GenerateData()
    {
        bool[][,] layerData = new bool[3][,];
        layerData[0] = GetFloorLayer();
        layerData[1] = GetWallLayer();
        layerData[2] = GetCeilingLayer();
        InteractableData[,] interactables = new InteractableData[ActualWidth, ActualHeight];
        HouseWorldData houseWorldData = new HouseWorldData(ActualWidth, ActualHeight, layerData, interactables, worldScene);
        houseWorldData.SpawnPoint = new Vector2(doorPosition + 3, 1);
        return houseWorldData;
    }

    private bool[,] GetFloorLayer()
    {
        var layer = new bool[ActualWidth, ActualHeight];
        for(int x = 1; x <= roomWidth; x++)
        for (int y = 2; y <= roomHeight - 2; y++)
            layer[x, y] = true;
        layer[doorPosition, 1] = true;
        return layer;
    }
    
    private bool[,] GetWallLayer()
    {
        var layer = new bool[ActualWidth, ActualHeight];
        for(int x = 1; x <= roomWidth; x++)
        for (int y = roomHeight - 1; y < roomHeight + 1; y++)
            layer[x, y] = true;
        return layer;
    }
    
    private bool[,] GetCeilingLayer()
    {
        var layer = new bool[ActualWidth, ActualHeight];
        // Tilemap helpers
        for (int x = 1; x <= roomWidth; x++)  layer[x, roomHeight + 2] = true;
        for (int y = 2; y <= roomHeight; y++) layer[roomWidth + 2, y] = true;
        for (int x = 0; x <= roomWidth + 1; x++)
        {
            layer[x, 1] = true;
            layer[x, roomHeight + 1] = true;
        }
        for (int y = 1; y <= roomHeight + 1; y++)
        {
            layer[0, y] = true;
            layer[roomWidth + 1, y] = true;
        }

        layer[doorPosition, 1] = false;
        layer[doorPosition - 1, 0] = true;
        layer[doorPosition, 0] = true;
        layer[doorPosition + 1, 0] = true;
        return layer;
    }
}