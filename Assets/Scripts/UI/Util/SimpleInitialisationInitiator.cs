using UnityEngine;

public class SimpleInitialisationInitiator : MonoBehaviour
{
    [Header("INeedsInitialisation")]
    public Component componentToInit;

    private void Awake()
    {
        if (componentToInit is INeedsInitialisation target)
        {
            target.Init();
        }
    }
}
