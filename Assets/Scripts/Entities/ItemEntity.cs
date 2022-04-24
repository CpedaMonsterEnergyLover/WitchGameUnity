using UnityEngine;

public class ItemEntity : Entity
{
    public new Rigidbody2D rigidbody;
    public SpriteRenderer spriteRenderer;

    public bool isMergeTarget;
    public bool isDroppedByPlayer;
    private bool isPickable = true;

    // Public fields
    public new ItemEntitySaveData SaveData => (ItemEntitySaveData) saveData;

    protected override void InitSaveData(EntityData origin)
    {
        base.InitSaveData(origin);
    }

    protected override void Start()
    {
        base.Start();
        spriteRenderer.sprite = SaveData.item.Data.icon;
        isPickable = false;

        Invoke(nameof(AllowPick), 0.5f);
        
        if(!isDroppedByPlayer) return;
        if(GetSameItemInRadius(0.8f, out ItemEntity sameItem))
        {
            sameItem.isMergeTarget = true;
            rigidbody.AddForce(
                (sameItem.transform.position - transform.position).normalized 
                * 35);
        } else {
            rigidbody.AddForce(Vector2.left * 
                               Random.Range(25, 35) * 
                               PlayerController.Instance.lookDirection);
        }
    }


    private void AllowPick() => isPickable = true;

    private bool GetSameItemInRadius(float radius, out ItemEntity sameItemEntity)
    {
        Collider2D[] results = new Collider2D[32];
        int size = Physics2D.OverlapCircleNonAlloc(transform.position, radius, 
             results, 1 << LayerMask.NameToLayer("ItemTrigger"));
        
        sameItemEntity = null;
        
        for(int i = 0; i < size; i++)
        {
            if (results[i].gameObject.TryGetComponent(out ItemEntity itemEntity) && 
                itemEntity.SaveData.item.Compare(SaveData.item) && itemEntity.IsStackable())
            {
                sameItemEntity = sameItemEntity is null ? itemEntity :
                    itemEntity.DistanceFromPlayer < sameItemEntity.DistanceFromPlayer ? itemEntity : 
                    sameItemEntity;
            }
        }

        return sameItemEntity is not null;
    }
    
    private bool IsStackable()
    {
        return SaveData.item.Data.maxStack > 1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && isPickable)
        {
            SaveData.amount -= WindowManager.Get<InventoryWindow>(WindowIdentifier.Inventory)
                .AddItem(SaveData.item.Data.identifier, SaveData.amount, true);
            if(SaveData.amount == 0) Kill();
        } 
        else if (other.gameObject.TryGetComponent(out ItemEntity itemEntity)) 
        {
            if(isMergeTarget || 
               !itemEntity.SaveData.item.Compare(SaveData.item) || 
               !itemEntity.IsStackable()) return;
            
            itemEntity.SaveData.amount += SaveData.amount;
            Kill();
        }
    }

}
