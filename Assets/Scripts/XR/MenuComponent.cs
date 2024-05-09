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

    public override void Grab(HandInteract interact)
    {
        matchedTrasform = interact.GetHandTransform();
        posOffset = interact.GetHandTransform().position - transform.position;
        rotOffset = interact.GetHandTransform().eulerAngles - transform.eulerAngles;
        held = true;
        OnUIGrabbed?.Invoke(this, interact);
    }

    public override void Drop(HandInteract interact)
    {
        matchedTrasform = null;
        held = false;
        OnUIDropped?.Invoke(this, interact);
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
