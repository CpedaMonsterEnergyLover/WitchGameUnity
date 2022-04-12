using System.Collections.Generic;
using System.Linq;

namespace TileLoading
{
    public class Cache <T> where T : class, ICacheable
    {
        private readonly List<T> _items = new();
        private readonly int _maxSize;
        public int Size => _items.Count;
        
        public Cache(int maxSize)
        {
            _maxSize = maxSize;
        }

        public void Add(T item)
        {
            if(Size >= _maxSize) Pop();
            item.GetCacheableItem.SetActive(false);
            item.IsLoaded = false;
            item.IsCached = true;
            _items.Add(item);
        }
        
        public void Remove(T item)
        {
            if(!_items.Contains(item)) return;
            _items.Remove(item);
            item.IsCached = false;
            item.IsLoaded = true;
        }

        private void Pop()
        {
            T popped = _items.First();
            popped.IsCached = false;
            popped.IsLoaded = false;
            popped.OnPopped();
            _items.Remove(popped);
        }

        public void Clear()
        {
            foreach (T cacheable in _items)
            {
                cacheable.IsCached = false;
                cacheable.IsLoaded = false;
                cacheable.OnPopped();
            }
            _items.Clear();
        }
    }
}