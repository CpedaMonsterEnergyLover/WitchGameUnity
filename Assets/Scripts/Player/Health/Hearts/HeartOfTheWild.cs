public class HeartOfTheWild : Heart
{
    public HeartOfTheWild(HeartType type) : base(
        GameCollection.Hearts.Get("heart_of_the_wild"), type)
    {
    }
}
