using System.Linq;
using UnityEngine;

public class InventoryKeyManager : MonoBehaviour
{
    #region Singleton

    public static InventoryKeyManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    [SerializeField]
    private string hotbarSlotKeyDescription;
    [SerializeField]
    private string inventorySlotKeyDescription;
    [SerializeField]
    private string useItemSlotKeyDescription;
    public ItemSlot slotUnderCursor;
    
    public string HotbarSlotKeyDescription => hotbarSlotKeyDescription;
    public string InventorySlotKeyDescription => 
        inventorySlotKeyDescription + (slotUnderCursor.storedItem is IConsumable ? useItemSlotKeyDescription : string.Empty);
    
    #region UnityMethods

    // Update is called once per frame
    void Update()
    {
        if (slotUnderCursor is not null && Input.anyKeyDown)
        {
            slotUnderCursor.OnKeyDown();
        }
    }

    #endregion
    
}
