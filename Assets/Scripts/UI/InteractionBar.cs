using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InteractionBar : MonoBehaviour
{
    public InteractionController interactionController;
    public Image barImage;

    private float _duration;
    private Action _actionOnComplete;
    private InteractionControllerData _dataOnStart;
    
    private void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.position = Input.mousePosition;
    }

    private void OnEnable()
    {
        SubEvents();
        UpdatePosition();
    }

    private void OnDisable()
    {
        UnsubEvents();
    }

    private void StopInteraction()
    {
        _duration = 0.0f;
        _actionOnComplete = null;
        gameObject.SetActive(false);
        NewItemPicker.Instance.HideWhileInteracting = false;
    }

    public void StartInteraction(float duration, Action actionOnComplete)
    {
        if (duration == 0.0f)
        {
            actionOnComplete.Invoke();
        }
        else
        {
            _duration = duration;
            _actionOnComplete = actionOnComplete;
            gameObject.SetActive(true);
            NewItemPicker.Instance.HideWhileInteracting = true;
            StartCoroutine(FillRoutine(duration, actionOnComplete));   
        }
    }

    private void ContinueInteraction()
    {
        if (!_dataOnStart.Equals(interactionController.ForceUpdateData()))
        {
            StopInteraction();
        }
        else
        {
            StartInteraction(_duration, _actionOnComplete);
        }
    }

    private void SubEvents()
    {
        Inventory.ONInventoryOpened += StopInteraction;
    }

    private void UnsubEvents()
    {
        Inventory.ONInventoryOpened -= StopInteraction;
    }
    
    private IEnumerator FillRoutine(float duration, Action actionOnComplete)
    {
        float t = 0.0f;
        barImage.fillAmount = 0.0f;
        _dataOnStart = interactionController.Data;
        while ( t  < duration )
        {
            if (PlayerController.Instance.MovementInput != Vector2.zero ||
                Input.GetMouseButtonUp(0) ||
                !_dataOnStart.Equals(interactionController.Data))
            {
                StopInteraction();
                yield break;
            }
            t += Time.deltaTime;
            barImage.fillAmount = t / duration;
            yield return null;
        }
        actionOnComplete.Invoke();
        ContinueInteraction();
    }

}
