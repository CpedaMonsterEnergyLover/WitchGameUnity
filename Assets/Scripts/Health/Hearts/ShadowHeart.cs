public class ShadowHeart : Heart
{
    public ShadowHeart(HeartType type) : base(
        GameCollection.Hearts.GetHeart("shadow_heart"), type)
    {
    }
}
