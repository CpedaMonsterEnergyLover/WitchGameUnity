public class HeartOfTheWild : Heart
{
    public HeartOfTheWild(HeartType type) : base(
        GameCollection.Hearts.GetHeart("heart_of_the_wild"), type)
    {
    }
}
