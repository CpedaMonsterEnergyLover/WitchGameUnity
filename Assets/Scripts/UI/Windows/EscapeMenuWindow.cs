using System.Linq;

public class EscapeMenuWindow : BaseWindow
{
    private TemporaryDismissData _dismissData;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        _dismissData = new TemporaryDismissData()
            .Add(WindowManager.All.ToList<ITemporaryDismissable>())
            .Exclude(this)
            .HideAll();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _dismissData.ShowAll();
    }
}
