using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bossbar : BaseWindow
{
    public static Bossbar Instance;

    public override void Init() => Instance = this;

    public Text nameText;
    public Transform healthbarTransform;
    public GameObject healthbarPrefab;

    private List<Image> _healthbarImages;
    
    public void SetBoss(Boss boss)
    {
        // if(isActiveAndEnabled) return;
        nameText.text = boss.name;
        int stagesCount = boss.stages.Count;

        _healthbarImages = new List<Image>();
        for(int i = 0; i < stagesCount; i++)
        {
            var bar = Instantiate(healthbarPrefab, healthbarTransform).GetComponent<Image>();
            _healthbarImages.Insert(0, bar);
            bar.color = boss.stages[i].healthColor;
        }
        gameObject.SetActive(true);
    }

    public void RemoveStage(int stageIndex) => _healthbarImages[stageIndex].enabled = false;

    public void SetHealth(int value, int maxValue, int stageIndex) =>
        _healthbarImages[stageIndex].fillAmount = Mathf.Clamp01((float) value / maxValue);

    public void Kill()
    {
        gameObject.SetActive(false);
        foreach (Transform o in healthbarTransform) Destroy(o.gameObject);
    }
}
