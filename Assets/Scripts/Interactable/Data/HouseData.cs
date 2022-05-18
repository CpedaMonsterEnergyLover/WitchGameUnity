using UnityEngine;

[CreateAssetMenu(menuName = "Interactable/House")]
public class HouseData : InteractableData
{
    [Header("HouseData")] 
    [Range(3, 20)]
    public int roomWidth;
    [Range(3, 20)]
    public int roomHeight;
    [Range(2, 19)]
    public int doorPosition;

    public int ActualWidth => roomWidth + 3;
    public int ActualHeight => roomHeight + 4;
}