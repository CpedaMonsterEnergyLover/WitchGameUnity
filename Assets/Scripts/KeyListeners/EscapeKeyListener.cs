using System.Collections.Generic;
using UnityEngine;

namespace KeyListeners
{
    public class EscapeKeyListener : KeyListener
    {
        public List<Component> toDisable;
        
        private TemporaryDismissData _dismissData;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                WindowManager.Toggle(WindowIdentifier.EscapeMenu);
                bool isPaused = WindowManager.IsActive(WindowIdentifier.EscapeMenu);

                _dismissData = isPaused ? 
                    new TemporaryDismissData().Add(toDisable).HideAll() : 
                    _dismissData?.ShowAll();
                
                Time.timeScale = isPaused ? 0 : 1;
            } 
        }
    }
}