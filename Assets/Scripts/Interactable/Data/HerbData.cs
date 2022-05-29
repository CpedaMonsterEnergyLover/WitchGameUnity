using System;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "Interactable/Herb")]
[Serializable]
public class HerbData : InteractableData
{
    [Header("Herb data")]
    public bool blockGrowth = true;

    public bool hasDrop;
    public ItemData itemDrop;

    /*
    [SerializeField] 
    [Tooltip("Сезон, в который появляются ростки растения")]
    public Season growthSeason;

    [Range(0.5f,3)]
    [Tooltip("Скорость увядания растения в количестве сезонов")]
    public float witherSpeed;
    */
    
    [SerializeField] [Tooltip("Сколько сезонов нужно растению чтобы полностью вырасти.")]
    [Range(0,6)]
    public float growthSpeed;
    
    /*
    [SerializeField] 
    [Tooltip("Какой лут падает с определенной стадии роста")]
    public HerbLootTable[] lootTable =
    {
        new HerbLootTable(GrowthStage.Sprout),
        new HerbLootTable(GrowthStage.Bush),
        new HerbLootTable(GrowthStage.Blossom),
        new HerbLootTable(GrowthStage.Grown),
        new HerbLootTable(GrowthStage.Decay),
    };
    */
    
    [SerializeField] 
    [Tooltip("Модельки")]
    public GrowthStageSprite[] growthSprites =
    {
        new GrowthStageSprite(GrowthStage.Sprout),
        new GrowthStageSprite(GrowthStage.Bush),
        new GrowthStageSprite(GrowthStage.Blossom),
        new GrowthStageSprite(GrowthStage.Grown),
        new GrowthStageSprite(GrowthStage.Decay),
    };



    /*
    public string[] GetLoot(GrowthStage stage) => 
        (int)stage < lootTable.Length ? lootTable[(int)stage].loot : Array.Empty<string>();

    public HerbLootTable GetLootTable(GrowthStage stage) => lootTable[(int) stage];
    */

    public int StageGrowthTime => (int) (growthSpeed * Timeline.SeasonLength / 4);

    public Sprite SpriteOfGrowthStage(GrowthStage stage) => growthSprites[(int) stage].sprite;

    
    
    /*
    #region Util

    private void OnValidate()
    {
        if (lootTable.Length != 5)
        {
            Debug.LogWarning("Don't change the 'lootTable' field's array size!");
            Array.Resize(ref lootTable, 5);
        }
        
        if (growthSprites.Length != 5)
        {
            Debug.LogWarning("Don't change the 'sprites' field's array size!");
            Array.Resize(ref growthSprites, 5);
        }
    }


    #endregion*/
}

[Serializable]
public struct GrowthStageSprite
{
    [ShowOnly]
    public GrowthStage growthStage;
    public Sprite sprite;

    public GrowthStageSprite(GrowthStage stage) : this()
    {
        growthStage = stage;
    }
}

[Serializable]
public struct HerbLootTable
{
    [ShowOnly]
    public GrowthStage growthStage;
    public string[] loot;
    public ItemData item;

    public HerbLootTable(GrowthStage growthStage) : this()
    {
        this.growthStage = growthStage;
    }

    public override string ToString()
    {
        if (loot.Length == 0) return "empty";
        StringBuilder toPrint = new StringBuilder();
        foreach (string s in loot)
        {
            toPrint.Append(s).Append(",");
        }

        toPrint.Remove(toPrint.Length - 1, 1);
        return toPrint.ToString();
    }
}

[Serializable]
public enum GrowthStage
{
    Sprout = 0,
    Bush = 1,
    Blossom = 2,
    Grown = 3,
    Decay = 4
}   