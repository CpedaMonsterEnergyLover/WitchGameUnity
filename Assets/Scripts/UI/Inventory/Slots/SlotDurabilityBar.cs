using UnityEngine;
using UnityEngine.UI;

public class SlotDurabilityBar : MonoBehaviour
{
    public Image barImage;

    public void SetActive(bool isActive) => gameObject.SetActive(isActive);

    public void UpdateDurability(IDamageableItem item)
    {
        gameObject.SetActive(true);
        barImage.fillAmount = (float) item.CurrentDamage / item.MaxDamage;
    }
}
