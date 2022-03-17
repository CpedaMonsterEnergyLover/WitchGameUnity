using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InteractionBar : MonoBehaviour
{
    public InteractionDataProvider interactionDataProvider;
    public Image barImage;

    private float _duration;
    private Action _actionOnComplete;
    private InteractionEventData _dataOnStart;
    private bool _isHand;
    
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

    public void StartInteraction(float duration, Action actionOnComplete, bool isHand)
    {
        _isHand = isHand;
        if (duration == 0.0f)
        {
            actionOnComplete.Invoke();
        }
        else
        {
            _dataOnStart = interactionDataProvider.Data;
            _duration = duration;
            _actionOnComplete = actionOnComplete;
            gameObject.SetActive(true);
            NewItemPicker.Instance.HideWhileInteracting = true;
            StartCoroutine(FillRoutine(duration, actionOnComplete));   
        }
    }

    private void ContinueInteraction()
    {
        if (!_dataOnStart.Equals(interactionDataProvider.ForceUpdateData()))
        {
            StopInteraction();
        }
        else
        {
            StartInteraction(_duration, _actionOnComplete, _isHand);
        }
    }

    private void OnWindowOpened(WindowIdentifier window)
    {
        if (window == WindowIdentifier.Inventory)
            StopInteraction();
    }
    
    private void SubEvents()
    {
        BaseWindow.ONWindowOpened += OnWindowOpened;
    }

    private void UnsubEvents()
    {
        BaseWindow.ONWindowOpened -= OnWindowOpened;
    }
    
    private IEnumerator FillRoutine(float duration, Action actionOnComplete)
    {
        float t = 0.0f;
        barImage.fillAmount = 0.0f;
        while ( t  < duration )
        {
            if (PlayerController.Instance.MovementInput != Vector2.zero ||
                Input.GetMouseButtonUp(0) ||
                !_dataOnStart.Equals(interactionDataProvider.Data) || !_isHand && !NewItemPicker.Instance.UseAllowed)
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
