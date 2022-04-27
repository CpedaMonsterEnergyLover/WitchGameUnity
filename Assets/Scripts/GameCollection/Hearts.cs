using System.Collections.Generic;
using UnityEngine;

namespace GameCollection
{
    public class Hearts : MonoBehaviour
    {
        public List<HeartData> heartDatas = new();

        private static readonly Dictionary<string, HeartData> Collection = new();
        public static HeartData Get(string id) => Collection[id];
        

        public void Init()
        {
            foreach (HeartData data in heartDatas)
            {
                Collection.Add(data.id, data);
            }
        }
    }
}