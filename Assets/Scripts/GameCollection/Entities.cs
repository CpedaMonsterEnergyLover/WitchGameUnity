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
            Collection.Clear();
            objects.ForEach(o =>
            {
                o.TryGetComponent(out Entity component);
                if (component is null)
                {
                    Debug.LogError(
                        $"Не удалось получить компонент {typeof(Entity)} на объекте {o.name} в коллекции 'Entities'");
                    return;
                }

                string id = component.Data.id;

                if (string.IsNullOrEmpty(id))
                {
                    Debug.LogError($"Пустой айди для объекта {component.Data.name} в коллекции 'Entities'");
                    return;
                }

                if (Collection.ContainsKey(id))
                {
                    Debug.LogError($"Айди {id} в коллекции 'Entities' уже присутствует");
                    return;
                }

                Collection.Add(component.Data.id, o);
            });
        }
    }
}