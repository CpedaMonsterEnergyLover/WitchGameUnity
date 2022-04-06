using UnityEngine;

namespace KeyListeners
{
    public class HotbarKeyListener : KeyListener
    {
        public HotbarWindow hotbarWindow;
        
        private void Update()
        {
            float wheel = Input.GetAxisRaw("Mouse ScrollWheel");

            if (Input.GetKeyDown(KeyCode.Q))
            {
                if(WindowManager.IsActive(WindowIdentifier.Inventory)) return;
                ItemSlot.OnDropItemButton(hotbarWindow.currentSelectedSlot.ReferredSlot);
            }
            else if(wheel != 0)
            {
                SelectFromWheel(wheel);
            }
            else
            {
                SelectFromKeyboard();
            }

        }

        private void SelectFromWheel(float wheel)
        {
            switch (wheel)
            {
                case < 0:
                    hotbarWindow.SelectSlot(hotbarWindow.selectedSlotIndex + 1);
                    break;
                case > 0:
                    hotbarWindow.SelectSlot(hotbarWindow.selectedSlotIndex - 1);
                    break;
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