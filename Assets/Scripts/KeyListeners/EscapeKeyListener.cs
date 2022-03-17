using UnityEngine;

namespace KeyListeners
{
    public class EscapeKeyListener : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                WindowManager.Toggle(WindowIdentifier.EscapeMenu);
                bool isPaused = WindowManager.IsActive(WindowIdentifier.EscapeMenu);
                Time.timeScale = isPaused ? 0 : 1;
            } 
        }
    }
}