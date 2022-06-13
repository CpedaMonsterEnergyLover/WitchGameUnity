using System;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "Interactable/Herb")]
[Serializable]
public class HerbData : InteractableData
{
    [Header("Herb data")]
    public bool blockGrowth = true;
    public string latinName;
    [Range(1,3)]
    public int tier;
    [Header("Herb growth season interval")]
    public SeasonInterval growthInterval;

    [Range(1,45), Header("Amount of days to reach Mature growth stage")]
    public int growthSpeed;

    public bool forceOneDrop;
    public ItemData forcedDrop;
    
    [SerializeField] 
    [Tooltip("Growth stage sprites")]
    public HerbLootTable[] loot =
    {
        new(HerbPart.Seed),
        new(HerbPart.Leaf),
        new(HerbPart.Flower),
        new(HerbPart.Fetus),
    };

    [SerializeField] 
    [Tooltip("Growth stage sprites")]
    public GrowthStageSprite[] growthSprites =
    {
        new(HerbGrowthStage.Seed),
        new(HerbGrowthStage.Sprout),
        new(HerbGrowthStage.Bush),
        new(HerbGrowthStage.Blossom),
        new(HerbGrowthStage.Mature),
        new(HerbGrowthStage.Old),
    };
    
    public Sprite SpriteOfGrowthStage(HerbGrowthStage stage) => growthSprites[(int) stage].sprite;
    public ItemData LootOfHerbPart(HerbPart herbPart) => loot[(int) herbPart].Item;
    
    private void OnValidate()
    {
        if (growthSprites.Length != 6 || loot.Length != 4)
        {
            Array.Resize(ref growthSprites, 6);
            Array.Resize(ref loot, 4);
        }
    }
}

[Serializable]
public struct GrowthStageSprite
{
    [ShowOnly]
    public HerbGrowthStage stage;
    public Sprite sprite;

    public GrowthStageSprite(HerbGrowthStage stage) : this()
    {
        this.stage = stage;
    }
}

[Serializable]
public struct HerbLootTable
{
    [ShowOnly]
    public HerbPart part;

    public bool hasItem;
    [SerializeField] private HerbDerivedItemData item;

    public HerbLootTable(HerbPart part) : this()
    {
        this.part = part;
    }

    public HerbDerivedItemData Item => hasItem ? item : null;
}


[Serializable]
public enum HerbGrowthStage
{
    Seed = 0,
    Sprout = 1,
    Bush = 2,
    Blossom = 3,
    Mature = 4,
    Old = 5
}

public enum HerbPart
{
    Seed = 0,
    Leaf = 1,
    Flower = 2,
    Fetus = 3
}