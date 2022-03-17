using UnityEngine;

[CreateAssetMenu(menuName = "Items/Furniture")]
public class PlaceableData : ItemData
{
    public GameObject prefab;
    public InteractableData interactable;
}
