using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandControllerInteract : HandInteract
{

    [SerializeField] protected InputActionProperty grab, interact;
    [SerializeField] protected Transform grabPoint;

    protected InteractableComponent currentInteractable;
    protected HandAnimator anim;

    // The hand transform

    // If this transform is not null, the player's hand model will snap to it and stay there
    protected Transform handOverride;

    // Default hand positions
    protected Vector3 defaultPos;
    protected Quaternion defaultRot;

    new protected void Awake()
    {
        base.Awake();

        anim = GetComponentInChildren<HandAnimator>();

        if(anim)
        {
            handTransform = anim.transform;
        } else
        {
            Debug.LogError("DBXR - No hand animator on hand!");
        }

        defaultPos = handTransform.localPosition;
        defaultRot = handTransform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {

        if (grab.action.WasPressedThisFrame())
        {
            InteractableComponent ic = GetNearestInteractable();
            if (ic != null)
            {
                currentInteractable = ic;
                anim.SetFistOverride(true);
                currentInteractable.Grabbed.Invoke(this);
            }
        }

        if (grab.action.WasReleasedThisFrame())
        {
            if (currentInteractable)
            {
                currentInteractable.Dropped.Invoke(this);
                currentInteractable = null;
                anim.SetFistOverride(false);
            }
        }

        if (interact.action.WasPressedThisFrame())
        {
            if (currentInteractable)
            {
                currentInteractable.InteractStarted.Invoke(this);
            }
        }

        if (interact.action.WasReleasedThisFrame())
        {
            if (currentInteractable)
            {
                currentInteractable.InteractStopped.Invoke(this);
            }
        }

        if (handOverride)
        {
            ClampToOverride();
        }

    }

    public InteractableComponent GetNearestInteractable()
    {

        Collider[] col = Physics.OverlapSphere(transform.position, DBXRResources.Main.InteractRadius, DBXRResources.Main.InteractLayerMask);

        if (col.Length > 0)
        {
            float closestDist = Mathf.Infinity;
            InteractableComponent closest = null;

            foreach (Collider c in col)
            {

                InteractableComponent ic = c.GetComponentInParent<InteractableComponent>();

                if (ic == null || !ic.IsGrabbable() || (ic.IsHeld() && ic is not TwoHandedInteractableComponent))
                    continue;

                float dist = Vector3.SqrMagnitude(c.transform.position - transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = ic;
                }
            }

            return closest;

        }
        else
        {
            return null;
        }

    }

    public void SetHandOverride(Transform t)
    {
        this.handOverride = t;
        if (!t)
        {
            handTransform.localPosition = defaultPos;
            handTransform.localRotation = defaultRot;
        }
        else
        {
            ClampToOverride();
        }
    }

    public void ClampToOverride()
    {
        Vector3 diff = (handOverride.position - grabPoint.position);
        handTransform.position += diff;
        handTransform.rotation = Quaternion.LookRotation(-handOverride.up, handOverride.right * transform.parent.localScale.x);
    }

    public Transform GetHandOverride()
    {
        return this.handOverride;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == DBXRResources.Main.InteractLayer)
        {
            InteractableComponent ic = other.GetComponent<InteractableComponent>();
            if (ic)
                ic.HandEntered.Invoke(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == DBXRResources.Main.InteractLayer)
        {
            InteractableComponent ic = other.GetComponent<InteractableComponent>();
            if (ic)
                ic.HandExited.Invoke(this);
        }
    }

}