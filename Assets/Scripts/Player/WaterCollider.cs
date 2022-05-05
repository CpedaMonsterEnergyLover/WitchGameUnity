using System.Collections;
using System.Collections.Generic;
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

    public void StartDash(float dashDuration)
    {
        _startDashPosition = transform.position;
        SetActive(false);
        StartCoroutine(StopDash(dashDuration));
    }

    private void DashEndedUpInVoid() { }
    
    private void DashEndedUpInWater()
    {
        waterDropsParticleSystem.Play();
        PlayerManager.Instance.PlayerSpriteRenderer.enabled = false;
        PlayerController.Instance.Stop();
        _dismissData = new TemporaryDismissData().Add(toDismiss).HideAll();

        ScreenFader.SetContinuation(() =>
        {
            TransferBack();
            _dismissData = _dismissData.ShowAll();
        });
        ScreenFader.StartFade();
    }
    

    
    private void TransferBack()
    {
        PlayerManager.Instance.PlayerSpriteRenderer.enabled = true;
        PlayerController.Instance.transform.position = _startDashPosition;
        cameraController.UpdatePosition();
        ScreenFader.StopFade();
    }
    
    private IEnumerator StopDash(float dashDuration)
    {
        yield return new WaitForSeconds(dashDuration);
        SetActive(true);
        yield return new WaitForSeconds(0.15f);
        var playerTile = PlayerManager.Instance.TilePosition;
        if (WorldManager.Instance.TryGetTopLayer(
            WorldManager.Instance.WorldData.GetTile(playerTile.x, playerTile.y), out WorldLayer worldLayer))
        {
            if (worldLayer.layerType == WorldLayerType.Water)
            {
                DashEndedUpInWater();
            }
        } 
        // Fall in void
        else {
            DashEndedUpInVoid();
        }
    }
}