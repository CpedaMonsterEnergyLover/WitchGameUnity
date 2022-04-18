using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleArenaBorder : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public ParticleSystem particles;
    public GameObject colliderGO;

    public void SetRadius(float newSize)
    {
        ParticleSystem.ShapeModule particleSystemShape = particles.shape;
        particleSystemShape.radius = newSize + 1;
        Transform colliderGOTransofrm = colliderGO.transform;
        Vector3 localPosition = colliderGOTransofrm.localPosition;
        localPosition.y = -newSize + 4;
        localPosition.z = -newSize + 4;
        colliderGOTransofrm.localPosition = localPosition;
    }

    public void SetActive(bool isActive) => gameObject.SetActive(isActive);
    
    void Update()
    {
        transform.LookAt(target);
        transform.Rotate(offset);
    }
}
