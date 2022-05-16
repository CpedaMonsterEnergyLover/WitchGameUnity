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
                ItemSlot.OnDropItemButton(hotbarWindow.SelectedSlot.ReferenceSlot);
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
            int selectedIndex = hotbarWindow.SelectedSlot.Index;
            switch (wheel)
            {
                case < 0:
                    hotbarWindow.SelectSlot(selectedIndex + 1);
                    break;
                case > 0:
                    hotbarWindow.SelectSlot(selectedIndex - 1);
                    break;
            }
        }

        private void SelectFromKeyboard()
        {
            for (int i = 0; i < 8; i++)
                if (Input.GetKeyDown((i + 1).ToString()))
                    hotbarWindow.SelectSlot(i);
        }
        
        
    }
}