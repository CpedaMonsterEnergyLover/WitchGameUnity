using UnityEngine;

public class EditableWorldLayer : WorldLayer
{
    public WorldLayerEditSettings layerEditSettings = new();
    
    public void Dig(Vector2Int position)
    {
        ItemEntity itemEntity = (ItemEntity) Entity.Create(new ItemEntitySaveData(layerEditSettings.dropItem, 1, 
            position + new Vector2(0.5f, 0.5f)));
        itemEntity.rigidbody.AddForce(Random.insideUnitCircle.normalized * 7.5f);
        tilemap.SetTile((Vector3Int) position, null);
        WorldManager.Instance.WorldData.GetTile(position.x, position.y).Layers[index] = false;
    }
}
