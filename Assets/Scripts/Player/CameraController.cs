using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    public bool following;
    private int treeMask;

    private void Awake()
    {
        treeMask = LayerMask.GetMask("Terrain");
    }

    // Update is called once per frame
    void Update()
    {
        if (following)
        {
            Vector3 position = transform.position;
            position = new Vector3(playerTransform.position.x, playerTransform.position.y + 0.5f, position.z);
            transform.position = position;
        }

    }
}
