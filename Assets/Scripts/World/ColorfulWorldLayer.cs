using UnityEngine;

public class ColorfulWorldLayer : WorldLayer
{
    [Header("Color settings")] 
    public Gradient colorGradient;
    public WorldNoiseMapIndex colorNoise;

    public Color[,] GetColors(GeneratorSettings generatorSettings,
        WorldNoiseData worldNoiseData)
    { 
        int width = generatorSettings.width;
        int height = generatorSettings.height;
        var colors = new Color[width, height];
        var noiseLayer = worldNoiseData.GetNoiseMap(colorNoise);
        for(int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            colors[x, y] = colorGradient.Evaluate(noiseLayer[x,y]);
        }

        return colors;
    }
}