using System;
using UnityEngine.UI;

public class InventoryTooltip : Tooltip
{
    public Text title;
    public Text subtitle;
    public Text description;


    public override void SetData(TooltipData data)
    {
        title.text = data.Title;
        subtitle.text = data.Subtitle;
        description.text = data.Description.Replace("\\n","\n");
    }

}

[Serializable]
public class TooltipData
{
    public readonly string Title;
    public readonly string Subtitle;
    public readonly string Description;

    public TooltipData(string newTitle = "", 
        string newSubtitle = "", 
        string newDesc = "")
    {
        Title = newTitle;
        Subtitle = newSubtitle;
        Description = newDesc;
    }
}

