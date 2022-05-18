using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class WaterCollider : MonoBehaviour
{
    public new Collider2D collider2D;
    public ParticleSystem waterDropsParticleSystem;
    public CameraController cameraController;
    public List<Component> toDismiss;

    private TemporaryDismissData _dismissData;
    private Vector3 _startDashPosition;

    private void SetActive(bool isActive) => collider2D.enabled = isActive;

    public async UniTaskVoid StartDash(float dashDuration)
    {
        _startDashPosition = transform.position;
        SetActive(false);
        await StopDash(dashDuration);
    }

    private async UniTask DashEndedUpInVoid()
    {
        await UniTask.Yield(); 
    }
    
    private async UniTask DashEndedUpInWater()
    {
        waterDropsParticleSystem.Play();
        PlayerManager.Instance.SpriteRenderer.enabled = false;
        PlayerController.Instance.Stop();
        _dismissData = new TemporaryDismissData().Add(toDismiss).HideAll();

        await ScreenFader.Instance.StartFade();
        await TransferBack();
        _dismissData = _dismissData.ShowAll();
    }
    

    
    private async UniTask TransferBack()
    {
        PlayerManager.Instance.SpriteRenderer.enabled = true;
        PlayerController.Instance.transform.position = _startDashPosition;
        cameraController.UpdatePosition();
        await ScreenFader.Instance.StopFade();
    }
    
    private async UniTask StopDash(float dashDuration)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(dashDuration));
        SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(0.15f));
        var playerTile = PlayerManager.Instance.TilePosition;
        if (WorldManager.Instance.TryGetTopLayer(
            WorldManager.Instance.WorldData.GetTile(playerTile.x, playerTile.y), out WorldLayer worldLayer))
        {
            if (worldLayer.layerType == WorldLayerType.Water)
            {
                await DashEndedUpInWater();
            }
        } 
        // Fall in void
        else {
            await DashEndedUpInVoid();
        }
    }
}