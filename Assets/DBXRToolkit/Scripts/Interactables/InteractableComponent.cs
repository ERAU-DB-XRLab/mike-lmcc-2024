using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableComponent : MonoBehaviour
{
    [SerializeField] protected Vector3 pos, rot;
    [SerializeField] protected bool grabbable = true;

    protected Rigidbody rb;

    protected Transform cachedParent;

    protected bool held = false;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void Grab(HandInteract interact)
    {

        rb.isKinematic = true;

        cachedParent = transform.parent;
        transform.SetParent(interact.GetHandTransform(), true);
        transform.localPosition = pos;
        transform.localRotation = Quaternion.Euler(rot);

        held = true;

    }

    public virtual void Drop(HandInteract interact)
    {

        //transform.SetParent(null);
        transform.SetParent(cachedParent);

        rb.isKinematic = false;
        rb.velocity = interact.GetVelocity() * DBXRResources.Main.ThrowSpeedMultiplier;

        held = false;

    }

    public virtual void InteractStart(HandInteract interact)
    {
        //
    }

    public virtual void InteractStop(HandInteract interact)
    {
        //
    }

    public virtual void HandEntered(HandInteract interact)
    {
        //
    }

    public virtual void HandExited(HandInteract interact)
    {
        //
    }

    public virtual void PointerEntered(HandInteract interact)
    {
        //
    }

    public virtual void PointerExited(HandInteract interact)
    {
        //
    }

    public bool IsGrabbable()
    {
        return grabbable;
    }

    public bool IsHeld()
    {
        return held;
    }

    public Rigidbody GetRB()
    {
        return rb;
    }

}
