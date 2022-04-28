public class HumanHeart : Heart
{
    public HumanHeart(HeartType type) : base(
        GameCollection.Hearts.GetHeart("human_heart"), type)
    {
    }
}
