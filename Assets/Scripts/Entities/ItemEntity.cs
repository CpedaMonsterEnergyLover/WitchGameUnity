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

    protected override void Start()
    {
        base.Start();
        spriteRenderer.sprite = SaveData.Item.Data.icon;
        isPickable = false;

        Invoke(nameof(AllowPick), 1f);
        
        if(!isDroppedByPlayer) return;
        if(GetSameItemInRadius(0.8f, out ItemEntity sameItem))
        {
            sameItem.isMergeTarget = true;
            rigidbody.AddForce(
                (sameItem.transform.position - transform.position).normalized 
                * 35);
        } else {
            rigidbody.AddForce(Vector2.left * 
                               UnityEngine.Random.Range(25, 35) * 
                               PlayerController.Instance.lookDirection);
        }
    }


    private void AllowPick() => isPickable = true;

    private bool GetSameItemInRadius(float radius, out ItemEntity sameItemEntity)
    {
        Collider2D[] results = new Collider2D[8];
        int size = Physics2D.OverlapCircleNonAlloc(transform.position, radius, 
             results, 1 << LayerMask.NameToLayer("DroppedItems"));
        
        sameItemEntity = null;
        
        for(int i = 0; i < size; i++)
        {
            if (results[i].gameObject.TryGetComponent(out ItemEntity itemEntity) && 
                itemEntity.SaveData.Item.Compare(SaveData.Item) && itemEntity.IsStackable())
            {
                sameItemEntity = sameItemEntity is null ? itemEntity :
                    itemEntity.distanceFromPlayer < sameItemEntity.distanceFromPlayer ? itemEntity : 
                    sameItemEntity;
            }
        }

        return sameItemEntity is not null;
    }
    
    private bool IsStackable()
    {
        return SaveData.Item.Data.maxStack > 1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && isPickable)
        {
            SaveData.Amount -= Inventory.Instance.AddItem(SaveData.Item.Data.identifier, SaveData.Amount, true);
            if(SaveData.Amount == 0) Kill();
        } 
        else if (other.gameObject.TryGetComponent(out ItemEntity itemEntity)) 
        {
            if(isMergeTarget || 
               !itemEntity.SaveData.Item.Compare(SaveData.Item) || 
               !itemEntity.IsStackable()) return;
            
            itemEntity.SaveData.Amount += SaveData.Amount;
            Kill();
        }
    }

}
