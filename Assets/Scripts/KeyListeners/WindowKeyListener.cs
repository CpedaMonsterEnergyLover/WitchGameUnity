﻿using UnityEngine;

namespace KeyListeners
{
    public class WindowKeyListener : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                WindowManager.Toggle(WindowIdentifier.Inventory);
            } else if (Input.GetKeyDown(KeyCode.C))
            {
                WindowManager.Toggle(WindowIdentifier.Crafting);
            }
        }
    }
}