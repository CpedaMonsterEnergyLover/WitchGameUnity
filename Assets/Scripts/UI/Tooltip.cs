using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    #region Singleton

    public static Tooltip Instance;

    private void Awake()
    {
        if (Instance is null) Instance = this;
        else Debug.LogError("Found multiple instances of tooltip");
        SetEnabled(false);
    }

    #endregion

    public RectTransform panelRect;
    public Text title;
    public Text subtitle;
    public Text description;
    
    public void SetEnabled(bool isEnabled)
    {
        if (isEnabled) Instance.UpdatePosition();
        gameObject.SetActive(isEnabled);
    }
    
    public void SetData(TooltipData data)
    {
        Instance.title.text = data.Title;
        Instance.subtitle.text = data.Subtitle;
        Instance.description.text = data.Description;
        SetEnabled(true);
    }

    private void UpdatePosition()
    {
        Vector3 position = Input.mousePosition;
        position.z = 0;
        var sizeDelta = panelRect.sizeDelta;
        position.x += sizeDelta.x / 2 + 30;
        position.y -= sizeDelta.y / 2 + 30;
        transform.position = position;
    }
    
    // Update is called once per frame
    private void Update()
    {
        UpdatePosition();
    }
}


public class TooltipData
{
    public readonly string Title;
    public readonly string Subtitle;
    public readonly string Description;

    public TooltipData(string newTitle = "", string newSubtitle = "", string newDesc = "")
    {
        Title = newTitle;
        Subtitle = newSubtitle;
        Description = newDesc;
    }
}