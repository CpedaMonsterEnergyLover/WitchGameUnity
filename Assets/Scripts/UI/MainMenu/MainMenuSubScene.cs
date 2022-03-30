using UnityEngine;

public class MainMenuSubScene : MonoBehaviour
{
    public void GoHere()
    {
        MainMenuCamera.Instance.TransferTo(transform.position);
    }    
}
