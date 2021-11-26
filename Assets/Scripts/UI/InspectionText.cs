using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectionText : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
    }

    private void Awake()
    {
        enabled = false;
    }
}
