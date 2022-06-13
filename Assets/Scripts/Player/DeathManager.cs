using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeathManager : MonoBehaviour
{
    [SerializeField] private PlayerAnimationManager animator;
    [SerializeField] private List<Component> toDismiss;
    [SerializeField] private float forceAmount = 1;
    [SerializeField] private GlobalVolumeAnimator volumeAnimator;

    public delegate void DeathEvent();
    public static event DeathEvent ONPlayerDeath;
    
    
    
    public static TemporaryDismissData TemporaryDismissData { get; private set; }
    
    public void Die()
    {
        ONPlayerDeath?.Invoke();

        Bullet[] bullets = FindObjectsOfType<Bullet>();
        for (var i = 0; i < bullets.Length; i++)
        {
            Destroy(bullets[i].gameObject);
        }
        
        TemporaryDismissData = new TemporaryDismissData().Add(toDismiss).HideAll();
        DropInventory();
        animator.Die();
        WindowManager.Get<InventoryWindow>(WindowIdentifier.Inventory).SetActive(false);
        WindowManager.Get<CraftingWindow>(WindowIdentifier.Crafting).SetActive(false);
        WindowManager.Get<PlaceableWindow>(WindowIdentifier.Placeable).SetActive(false);
        StartCoroutine(PlayerForceRoutine());
        volumeAnimator.PlayDeath();

        async UniTaskVoid RespawnTask()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(3f));
            await ScreenFader.Instance.StartFade(0.33f);
            
            PlayerManager.Instance.SetPosition(WorldManager.Instance.WorldData.SpawnPoint);
            animator.Idle();
            volumeAnimator.Clear();
            PlayerManager.Instance.SetHunger(35);
            PlayerManager.Instance.AddHeart();
            
            await ScreenFader.Instance.StopFade();
            TemporaryDismissData = TemporaryDismissData?.ShowAll();
        }
        
        RespawnTask().Forget();
    }

    private static void DropInventory()
    {
        var inventory = WindowManager.Get<InventoryWindow>(WindowIdentifier.Inventory);
        foreach (var slot in inventory.slots)
        {
            if(!slot.HasItem) continue;
            Debug.Log($"Dropping {slot.storedItem.Data.name}");
            ItemEntity itemEntity = (ItemEntity)Entity.Create(
                new ItemEntitySaveData(Item.Create(slot.storedItem.SaveData), slot.storedAmount, 
                    PlayerManager.Instance.Pos2D));
             
            itemEntity.rigidbody.AddForce(Random.insideUnitCircle.normalized * 10f);
            slot.Clear();
        }
        
    }

    private IEnumerator PlayerForceRoutine()
    {
        Vector2 initialVelocity = PlayerController.Instance.MovementInput.normalized;
        float t = 0.0f;
        Rigidbody2D rb = PlayerController.Instance.rigidBody;
        while (t < 1f)
        {
            t += Time.deltaTime;
            float value = t / 1f;
            rb.velocity = initialVelocity * (1 - value) * forceAmount;
            yield return null;
        }
        rb.velocity = Vector2.zero;
    }
}
