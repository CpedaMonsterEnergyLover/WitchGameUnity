using UnityEngine;

public class WorldPointer : MonoBehaviour
{
    #region Singleton

    public static WorldPointer Instance;

    private void Awake()
    {
        if (Instance is null) Instance = this;
        else Debug.LogWarning("More than 1 instance of WorldPointer found");
    }

    #endregion

    public Camera playerCamera;
    public GameObject tileSelector;
    public Color selectionAllowColor;
    public Color selectionDenyColor;

    public void ConfirmSelection(bool isConfirmed)
    {
        tileSelector.GetComponent<SpriteRenderer>().color = isConfirmed ? selectionAllowColor : selectionDenyColor;
    }
    
    // Update is called once per frame
    /*void Update()
    {
        Vector3 targetPosition = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = 0;


        Vector3Int targetPositionInt = Vector3Int.FloorToInt(targetPosition);
        if (WorldManager.CoordsBelongsToWorld(targetPositionInt.x, targetPositionInt.y))
        {
            WorldTile tile = WorldManager.WorldData.GetTile(targetPositionInt.x, targetPositionInt.y);
            if (tile is not null)
            {
                Vector3 position = tile.position;
                position.x += 0.5f;
                position.y += 0.5f;
                tileSelector.transform.position = position;
                ConfirmSelection(!tile.HasInteractable);
            }
        }
    }*/
}
