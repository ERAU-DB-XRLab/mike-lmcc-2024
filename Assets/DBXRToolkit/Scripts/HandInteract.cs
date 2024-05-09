using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandInteract : MonoBehaviour
{

    // The hand transform
    protected Transform handTransform;
    protected Velocity velComponent;

    protected void Awake()
    {
        velComponent = GetComponent<Velocity>();
    }
    public Vector3 GetVelocity()
    {
        return velComponent.GetVelocity();
    }

    public Transform GetHandTransform()
    {
        return handTransform;
    }

}
