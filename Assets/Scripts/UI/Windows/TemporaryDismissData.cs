using System.Collections.Generic;
using UnityEngine;

public class TemporaryDismissData
{
    private readonly List<ITemporaryDismissable> _dismissedObjects 
        = new();

    public TemporaryDismissData Add(ITemporaryDismissable dismissable)
    {
        if (dismissable.IsActive) _dismissedObjects.Add(dismissable);
        return this;
    }

    public TemporaryDismissData Add(List<ITemporaryDismissable> dismissables)
    {
        dismissables.ForEach(dismissable =>
        {
            if (dismissable.IsActive) _dismissedObjects.Add(dismissable);
        });
        return this;
    }

    public TemporaryDismissData Add(List<Component> components)
    {
        components.ForEach(component =>
        {
            if (component is ITemporaryDismissable {IsActive: true} dismissable) _dismissedObjects.Add(dismissable);
        });
        return this;
    }
    
    public TemporaryDismissData Exclude(ITemporaryDismissable dismissable)
    {
        if (_dismissedObjects.Contains(dismissable))
        {
            _dismissedObjects.Remove(dismissable);
        }
        return this;
    }
    
    public TemporaryDismissData HideAll()
    {
        _dismissedObjects.ForEach(dismissable => dismissable.SetActive(false));
        return this;
    }

    public TemporaryDismissData ShowAll()
    {
        _dismissedObjects.ForEach(dismissable => dismissable?.SetActive(true));
        return null;
    }

}



