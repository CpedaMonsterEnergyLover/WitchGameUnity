using UnityEngine;

namespace TileLoading
{
    public interface ICacheable
    {
        public bool IsLoaded { get; set; }
        public bool IsCached { get; set; }
        public GameObject GetCacheableItem { get; }
        public void LeaveCache();
    }
}