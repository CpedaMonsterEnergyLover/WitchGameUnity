using System.Collections.Generic;
using UnityEngine;

namespace GameCollection
{
    public class Entities : MonoBehaviour
    {
        public List<GameObject> objects;

        private static readonly Dictionary<string, GameObject> Collection = new();

        public static GameObject Get(string id) => Collection[id];
        public static bool ContainsID(string id) => Collection.ContainsKey(id);

        public void Init()
        {
            Manager.MapCollection<Herb>(objects, Collection, "entities");
        }
    }
}