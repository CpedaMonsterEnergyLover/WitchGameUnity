using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameCollection
{
    
    public class Manager : MonoBehaviour
    {
        public Items items;
        public Entities entities;
        public Interactables interactables;
        public CraftingRecipies recipies;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void Init()
        {
            items.Init();
            entities.Init();
            interactables.Init();
            recipies.Init();
        }

        public static void MapCollection<T>(List<GameObject> objects, Dictionary<string, GameObject> dictionary, string collectionName) where T : Interactable
        {
            objects.ForEach(o =>
            {
                o.TryGetComponent(out T component);
                if (component is null)
                {
                    Debug.LogError($"Не удалось получить компонент {typeof(T)} на объекте {o.name} в коллекции {dictionary}");
                    return;
                }

                string id = component.Data.id;

                if (string.IsNullOrEmpty(id))
                {
                    Debug.LogError($"Пустой айди для объекта {component.Data.name} в коллекции {collectionName}");
                    return;
                }
                
                if (dictionary.ContainsKey(id))
                {
                    Debug.LogError($"Айди {id} в коллекции {collectionName} уже присутствует");
                    return;
                }
                
                dictionary.Add(component.Data.id, o);
            });
        }
    }
    
    
}