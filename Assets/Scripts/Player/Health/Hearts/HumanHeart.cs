public class HumanHeart : Heart
{
    public HumanHeart(HeartType type) : base(
        GameCollection.Hearts.Get("human_heart"), type)
    {
    }
}
