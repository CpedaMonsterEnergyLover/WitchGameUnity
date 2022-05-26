using System.Collections.Generic;
using UnityEngine;

namespace GameCollection
{
    public class Biomes : MonoBehaviour
    {
        [SerializeField] private List<global::Biomes> objects;

        public void Init()
        {
            foreach (global::Biomes list in objects)
            {
                list.InitSpawnEdges();
            }
        }
    }
}