using UnityEngine;

namespace KeyListeners
{
    public class EscapeKeyListener : KeyListener
    {
        
        private TemporaryDismissData _dismissData;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                WindowManager.Toggle(WindowIdentifier.EscapeMenu);
            } 
        }
    }
}