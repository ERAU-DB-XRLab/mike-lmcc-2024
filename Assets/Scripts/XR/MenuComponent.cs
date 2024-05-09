using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuComponent : InteractableComponent
{
    public delegate void UIGrabbed(MenuComponent component, HandInteract interact);
    public event UIGrabbed OnUIGrabbed;
    public delegate void UIReleased(MenuComponent component, HandInteract interact);
    public event UIReleased OnUIDropped;

    private Transform matchedTrasform;
    private Vector3 posOffset;
    private Vector3 rotOffset;

    private const float centralBoxOffset = -315f;

    public override void Grab(HandInteract interact)
    {
        matchedTrasform = interact.GetHandTransform();
        posOffset = interact.GetHandTransform().position - transform.position;
        rotOffset = interact.GetHandTransform().eulerAngles - transform.eulerAngles;
        held = true;
        OnUIGrabbed?.Invoke(this, interact);

        InvertGrabPoint(false);
    }

    public override void Drop(HandInteract interact)
    {
        matchedTrasform = null;
        held = false;
        OnUIDropped?.Invoke(this, interact);
    }

    public void InvertGrabPoint(bool invert)
    {
        transform.GetChild(0).SetLocalPositionAndRotation(invert ? Vector3.up * -centralBoxOffset : Vector3.up * centralBoxOffset, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (held && matchedTrasform != null)
        {
            transform.position = matchedTrasform.position - posOffset;
            //transform.eulerAngles = matchedTrasform.eulerAngles + rotOffset;
        }
    }
}
