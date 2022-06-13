using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class Herb : Interactable
{
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Animator animator;
    public new HerbData Data => (HerbData) data;
    public new HerbSaveData SaveData => (HerbSaveData) saveData;

    private CancellationTokenSource _currentTokenSource;
    
    protected override void InitSaveData(InteractableData origin)
    {
        saveData = new HerbSaveData(origin)
        {
            stageDuration = (Data.growthSpeed * 24 * 60) / 4,
            growthStage = (HerbGrowthStage) Random.Range(2, 4),
            initialized = true
        };
    }

    private void OnDisable()
    {
        _currentTokenSource?.Cancel();
        Timeline.ONSeasonStart -= OnEndSeasonStart;
    }

    public override void OnTileLoad(WorldTile loadedTile)
    {
        base.OnTileLoad(loadedTile);
        if(Data.blockGrowth) return;
        UpdateSprite();
        if(SaveData.growthStage < HerbGrowthStage.Mature) 
            GrowProcess().Forget();
        else if(SaveData.growthStage == HerbGrowthStage.Old)
            Timeline.ONSeasonStart += OnEndSeasonStart;
    }

    private async UniTask GrowProcess()
    {
        _currentTokenSource?.Cancel();
        _currentTokenSource = new CancellationTokenSource();
        CancellationToken token = _currentTokenSource.Token;   
        while (true)
        {
            SaveData.age += 1;
            if (SaveData.age > SaveData.stageDuration)
            {
                Grow();
            }
            await UniTask.Delay(TimeSpan.FromSeconds(Timeline.MinuteDuration),
                cancellationToken: token);
        }
    }
    
    private void Grow()
    {
        if (SaveData.growthStage < HerbGrowthStage.Mature)
        {
            SaveData.growthStage++;
            SaveData.age = 0;
            if(SaveData.growthStage == HerbGrowthStage.Mature)
                _currentTokenSource?.Cancel();
        }
        AnimateGrow();
    }

    private void OnEndSeasonStart(int season)
    {
        if((Season) season == Data.growthInterval.End) Wither();
    }
    
    private void Wither()
    {
        SaveData.growthStage = HerbGrowthStage.Old;
        AnimateGrow();
    }
    
    public override void Interact(float value = 1.0f)
    {
        base.Interact(value);
        List<ItemStack> drops = new();
        if(!Data.forceOneDrop)
            switch (SaveData.growthStage)
            {
                case HerbGrowthStage.Seed:
                    drops.Add(new ItemStack(Data.LootOfHerbPart(HerbPart.Seed), 1));
                    break;
                case HerbGrowthStage.Sprout:
                    break;
                case HerbGrowthStage.Bush:
                    drops.Add(new ItemStack(Data.LootOfHerbPart(HerbPart.Leaf), Random.Range(1, 3)));
                    break;
                case HerbGrowthStage.Blossom:
                    drops.Add(new ItemStack(Data.LootOfHerbPart(HerbPart.Flower), 1));
                    drops.Add(new ItemStack(Data.LootOfHerbPart(HerbPart.Leaf), Random.Range(2, 4)));
                    break;
                case HerbGrowthStage.Mature:
                    drops.Add(new ItemStack(Data.LootOfHerbPart(HerbPart.Leaf), Random.Range(3, 5)));
                    drops.Add(new ItemStack(Data.LootOfHerbPart(HerbPart.Fetus), 1));
                    break;
                case HerbGrowthStage.Old:
                    drops.Add(new ItemStack(Data.LootOfHerbPart(HerbPart.Seed), 2));
                    drops.Add(new ItemStack(GameCollection.Items.Get("dry_grass"), Random.Range(3, 6)));
                    break;
            }
        else
        {
            drops.Add(new ItemStack(Data.forcedDrop, Random.Range(2, 5)));
        }
        foreach (ItemStack itemStack in drops)
        {
            if (itemStack.item is not null)
            {
                PlayerManager.Instance.AddItem(itemStack);
            }
        }
        Kill();
    }

    // Used by animator
    public void UpdateSprite()
    {
        spriteRenderer.sprite = Data.SpriteOfGrowthStage(SaveData.growthStage);
    }

    private void AnimateGrow()
    {
        animator.Play("HerbGrow");
    }

}
