using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Vein : Interactable
{
    public new VeinData Data => (VeinData) data;

    public new VeinSaveData SaveData => (VeinSaveData) saveData;

    private SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        ChangeSpriteOnHealth();
    }

    protected override void InitSaveData(InteractableData origin)
    {
        saveData = new VeinSaveData(origin);
    }

    public override void Interact(float value = 1)
    {
        SaveData.health--;
        ChangeSpriteOnHealth();
        Debug.Log($"{SaveData.health}");
        if (SaveData.health <= 0)
        {
            Destroy();
        }
    }

    private void ChangeSpriteOnHealth()
    {
        int spriteIndex = (SaveData.health - 1) / 2;
        _renderer.sprite = Data.sprites[spriteIndex];
    }
}



