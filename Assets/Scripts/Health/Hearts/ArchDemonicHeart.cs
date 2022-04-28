public class ArchDemonicHeart : Heart
{
    public ArchDemonicHeart(HeartType type) : base(
        GameCollection.Hearts.GetHeart("archdemonic_heart"), type)
    { }
    
    public ArchDemonicHeart(HeartData data, HeartType type) : base(data, type)
    { }

    public override void OnCreated()
    {
    }
}
