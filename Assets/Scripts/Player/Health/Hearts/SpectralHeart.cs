public class SpectralHeart : Heart
{
    public SpectralHeart(HeartType type) : base(
        GameCollection.Hearts.Get("spectral_heart"), type)
    {
    }
}
