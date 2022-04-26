using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InteractionBar : MonoBehaviour
{
    public InteractionDataProvider interactionDataProvider;
    public Image barImage;

    private InteractionEventData _dataOnStart;

    private void OnEnable()
    {
        SubEvents();
    }

    private void OnDisable()
    {
        UnsubEvents();
    }

    private void StopInteraction()
    {
        gameObject.SetActive(false);
        NewItemPicker.Instance.HideWhileInteracting = false;
    }

    public void StartInteraction(
        float duration, 
        Action actionOnComplete, 
        bool isHand,
        InteractionFilter filter)
    {
        if (duration == 0.0f)
        {
            actionOnComplete.Invoke();
        }
        else
        {
            _dataOnStart = InteractionDataProvider.Data;
            gameObject.SetActive(true);
            NewItemPicker.Instance.HideWhileInteracting = true;
            StartCoroutine(FillRoutine(duration, actionOnComplete, isHand, filter));   
        }
    }

    private void ContinueInteraction(float duration, Action actionOnComplete, bool isHand, InteractionFilter filter)
    {
        if (filter.stopOnTargetChange &&
            !_dataOnStart.Equals(InteractionDataProvider.ForceUpdateData()))
            StopInteraction();
        else
            StartInteraction(duration, actionOnComplete, isHand, filter);
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
    
    private IEnumerator FillRoutine(float duration, Action actionOnComplete, bool isHand, InteractionFilter filter)
    {
        float t = 0.0f;
        barImage.fillAmount = 0.0f;
        while ( t  < duration )
        {
            bool stopOnMove = filter.stopOnMove && PlayerController.Instance.MovementInput != Vector2.zero;
            bool notHandAndUseNotAllowed = !isHand && !NewItemPicker.Instance.UseAllowed;
            bool stopOnTargetChange = filter.stopOnTargetChange && !_dataOnStart.Equals(InteractionDataProvider.Data);
            
            if (stopOnMove ||
                Input.GetMouseButtonUp(0) ||
                stopOnTargetChange || 
                notHandAndUseNotAllowed)
            {
                StopInteraction();
                yield break;
            }
            t += Time.deltaTime;
            barImage.fillAmount = t / duration;
            yield return null;
        }
        actionOnComplete.Invoke();
        ContinueInteraction(duration, actionOnComplete, isHand, filter);
    }

}

public readonly struct InteractionFilter
{
    public readonly bool stopOnMove;
    public readonly bool stopOnTargetChange;
    

    public InteractionFilter(bool stopOnMove, bool stopOnTargetChange)
    {
        this.stopOnMove = stopOnMove;
        this.stopOnTargetChange = stopOnTargetChange;
    }
}
