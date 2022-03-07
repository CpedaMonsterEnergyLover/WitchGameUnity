using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

namespace GameCollection
{
    public class Interactables : MonoBehaviour
    {
        public List<GameObject> herbs;
        public List<GameObject> trees;
        public List<GameObject> other;

        private static readonly Dictionary<string, GameObject> Collection = new();

        public static GameObject Get(string id) => Collection[id];
        public static bool ContainsID(string id) => Collection.ContainsKey(id);
        
        public void Init()
        {
            Manager.MapCollection<Herb>(herbs, Collection, "herbs");
            Manager.MapCollection<WoodTree>(trees, Collection, "trees");
            Manager.MapCollection<Interactable>(other, Collection, "others");
        }
    }
}