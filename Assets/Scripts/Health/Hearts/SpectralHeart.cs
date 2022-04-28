public class SpectralHeart : Heart
{
    public SpectralHeart(HeartType type) : base(
        GameCollection.Hearts.GetHeart("spectral_heart"), type)
    {
    }
}
