using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public RectTransform panelRect;
    public Text title;
    public Text subtitle;
    public Text description;

    private static Tooltip instance;

    public static void SetEnabled(bool isEnabled)
    {
        instance.panelRect.gameObject.SetActive(isEnabled);
    }

    public static void SetData(TooltipData data)
    {
        instance.title.text = data.Title;
        instance.subtitle.text = data.Subtitle;
        instance.description.text = data.Description;
        instance.UpdatePosition();
        SetEnabled(true);
    }
    
    private void Awake()
    {
        if (instance is null) instance = this;
        else Debug.LogError("Found multiple instances of tooltip");
        SetEnabled(false);
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
    void Update()
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