using UnityEngine;
using UnityEngine.UI;

public class DurabilityBar : MonoBehaviour
{
    public Image barImage;


    public void SetActive(bool isActive) => gameObject.SetActive(isActive);

    public void UpdateDurability(Instrument item)
    {
        gameObject.SetActive(true);
        barImage.fillAmount = (float) item.SaveData.Durability / item.Data.maxDurability;
    }
}
