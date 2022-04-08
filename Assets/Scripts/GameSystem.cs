using UnityEngine;

namespace DefaultNamespace
{
    public class GameSystem : MonoBehaviour
    {

        private static GameSystem Instance;

        private void Awake()
        {
            if (Instance is null)
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