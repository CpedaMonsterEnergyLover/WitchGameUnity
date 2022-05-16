using UnityEngine;

public class House : Interactable, IPlayerReceiver
{
    public new HouseData Data => (HouseData) data;
     
    public void OnReceivePlayer()
    {
        Debug.Log("Игрок входит в дом");
        Data.worldScene.LoadFromAnotherWorld(Data.worldScene.worldParts.IndexOf(Data));
    }

    public void OnPlayerExitReceiver()
    { }
}