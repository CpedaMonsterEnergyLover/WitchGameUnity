public class ArchDemonicHeart : Heart
{
    public ArchDemonicHeart(HeartType type) : base(
        GameCollection.Hearts.Get("archdemonic_heart"), type)
    { }
    
    public ArchDemonicHeart(HeartData data, HeartType type) : base(data, type)
    { }

    public override void OnCreated()
    {
        var heartContainer = PlayerManager.Instance.gameObject.GetComponent<HeartContainer>();
        heartContainer.PopAll(heartContainer.FindAllByOrigin(HeartOrigin.Human));
    }
}
