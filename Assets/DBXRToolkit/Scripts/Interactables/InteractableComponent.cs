using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableComponent : MonoBehaviour
{

    [HideInInspector]
    public UnityEvent<HandInteract> Grabbed, Dropped, InteractStarted, InteractStopped, HandEntered, HandExited, PointerEntered, PointerExited, RayEntered, RayExited;

    [SerializeField] protected Vector3 pos, rot;
    [SerializeField] protected bool grabbable = true;
    
    protected bool held = false;
    protected Rigidbody rb;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Grabbed.AddListener(Grab);
        Dropped.AddListener(Drop);
    }

    public virtual void Grab(HandInteract interact)
    {

        rb.isKinematic = true;

        transform.SetParent(interact.GetHandTransform(), true);
        transform.localPosition = pos;
        transform.localRotation = Quaternion.Euler(rot);

        held = true;

    }

    public virtual void Drop(HandInteract interact)
    {

        transform.SetParent(null);

        rb.isKinematic = false;
        rb.velocity = interact.GetVelocity() * DBXRResources.Main.ThrowSpeedMultiplier;

        held = false;

    }

    public Rigidbody GetRB()
    {
        return rb;
    }

    public bool IsGrabbable()
    {
        return grabbable;
    }

    public bool IsHeld()
    {
        return held;
    }

}