using System.Collections.Generic;
using UnityEngine;

namespace GameCollection
{
    public class Hearts : MonoBehaviour
    {
        public List<HeartData> heartDatas = new();
        public List<HeartEffectData> effectDatas = new();

        private static readonly Dictionary<string, HeartData> HeartsCollection = new();
        private static readonly Dictionary<string, HeartEffectData> EffectsCollection = new();
        public static HeartData GetHeart(string id) => HeartsCollection[id];
        public static HeartEffectData GetEffect(string id) => EffectsCollection[id];
        

        public void Init()
        {
            HeartsCollection.Clear();
            EffectsCollection.Clear();
            foreach (HeartData data in heartDatas) HeartsCollection.Add(data.id, data);
            foreach (HeartEffectData data in effectDatas) EffectsCollection.Add(data.id, data);
        }
    }
}