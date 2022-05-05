﻿public class DemonicHeart : Heart
{
    public DemonicHeart(HeartType type) : base(
        GameCollection.Hearts.GetHeart("demonic_heart"), type)
    {
    }

    public DemonicHeart(HeartData data, HeartType type) : base(data, type)
    {
    }
}