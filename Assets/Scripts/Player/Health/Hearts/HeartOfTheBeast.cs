public class HeartOfTheBeast : Heart
{
    public HeartOfTheBeast(HeartType type) : base(
        GameCollection.Hearts.Get("heart_of_the_beast"), type)
    {
    }
}
