using System.Collections.Generic;
using UnityEngine;

namespace GameCollection
{
    public class Items : MonoBehaviour
    {
        public List<ItemData> objects = new ();

        private static readonly Dictionary<string, ItemData> Collection = new();

        public static ItemData Get(string id) => Collection[id];
        public static bool ContainsID(string id) => Collection.ContainsKey(id);

        public void Init()
        {
            Collection.Clear();
            objects.ForEach(o => Collection.Add(o.identifier.id, o));
        }
    }
}