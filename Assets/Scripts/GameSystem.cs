using UnityEngine;

namespace DefaultNamespace
{
    public class GameSystem : MonoBehaviour
    {
        public GameCollection.Manager collectionManager;
        
        private static GameSystem Instance;

        private void Awake()
        {
            if (Instance != this)
            {
                Instance = this;
            }
            else
            {
                GameObject o;
                (o = gameObject).SetActive(false);
                DestroyImmediate(o);
            }
        }
    }
}