using UnityEngine;

public class House : Interactable, IPlayerReceiver
{
    public void OnReceivePlayer()
    {
        Debug.Log("Игрок входит в дом");
    }

    public void OnPlayerExitReceiver()
    { }
}