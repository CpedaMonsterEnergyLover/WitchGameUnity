using UnityEngine;

namespace KeyListeners
{
    public class HotbarKeyListener : KeyListener
    {
        public HotbarWindow hotbarWindow;
        
        private void Update()
        {
            SelectFromWheel();
            SelectFromKeyboard();
        }

        private void SelectFromWheel()
        {
            // Mouse wheel
            float wheel = Input.GetAxisRaw("Mouse ScrollWheel");
            if (wheel < 0)
            {
                hotbarWindow.SelectSlot(hotbarWindow.selectedSlotIndex + 1);
            }
            else if (wheel > 0)
            {
                hotbarWindow.SelectSlot(hotbarWindow.selectedSlotIndex - 1);
            }
        }

        private void SelectFromKeyboard()
        {
            // Keyboard
            for (int i = 0; i < 8; i++)
                if (Input.GetKeyDown((i + 1).ToString()))
                    hotbarWindow.SelectSlot(i);
        }
    }
}