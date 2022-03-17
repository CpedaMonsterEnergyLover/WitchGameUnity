using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/CraftRecipe")]
public class CraftingRecipe : ScriptableObject
{
    public SkillCategory category;
    public ItemStack result;
    [Header("Необходимые инструменты")]
    public List<ItemData> instruments = new();
    [Header("Станции крафта")]
    public List<InteractableData> craftStations = new();
    [Header("Компоненты крафта")]
    public List<ItemStack> components = new();
}