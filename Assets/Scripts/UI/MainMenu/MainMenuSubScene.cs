using UnityEngine;

public class MainMenuSubScene : MonoBehaviour
{
    public TransferAxis transferAxis;
    
    public enum TransferAxis
    {
        Horizontal,
        Vertical
    }

    private void Awake()
    {
        gameObject.SetActive(false);
    }


    public void GoHere(bool vertical)
    {
        MainMenuCamera.Instance.TransferTo(this, vertical);
        /*if(transferAxis is TransferAxis.Horizontal)
            MainMenuCamera.Instance.TransferHorizontal(transform.position);
        else
            MainMenuCamera.Instance.TransferVertical(transform.position);*/
    }    
}
