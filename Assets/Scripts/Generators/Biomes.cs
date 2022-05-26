using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Biomes list")]
public class Biomes : ScriptableObject
{
    public List<Biome> list = new();

    public void InitSpawnEdges()
    {
        list.ForEach(biome => biome.Init());
    }
}
