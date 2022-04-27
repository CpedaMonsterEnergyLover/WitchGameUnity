public class ShadowHeart : Heart
{
    public ShadowHeart(HeartType type) : base(
        GameCollection.Hearts.Get("shadow_heart"), type)
    {
    }
}
