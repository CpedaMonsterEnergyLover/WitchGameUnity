using UnityEngine;

[CreateAssetMenu(menuName = "Items/Food")]
[System.Serializable]
public class FoodData : ItemData
{
    [Header("Питательность еды")]
    public float saturation;
}