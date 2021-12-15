using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private Inventory _inventory;
    
    // Start is called before the first frame update
    private void Start()
    {
        _inventory = Inventory.Instance;
        _inventory.ONItemChanged += UpdateUI;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateUI()
    {
        Debug.Log("UPDATING UI");
    }
}
