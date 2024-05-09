using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Hands.Gestures;

[RequireComponent(typeof(LineRenderer))]
public class HandTrackingInteract : HandInteract
{


    [SerializeField] private XRHandShape pinch;
    [SerializeField] private Transform aimRef, grabPoint;

    private XRHandTrackingEvents events;
    private LineRenderer l;
    private InteractableComponent currentInteractable, grabbedInteractable;

    private bool grabbing = false;

    new void Awake()
    {
        base.Awake();
        
        l = GetComponent<LineRenderer>();
        l.positionCount = 2;

        events = GetComponent<XRHandTrackingEvents>();
        events.jointsUpdated.AddListener(JointsUpdated);

        handTransform = grabPoint;
    }

    public void JointsUpdated(XRHandJointsUpdatedEventArgs args)
    {
        if(pinch.CheckConditions(args))
        {
            if(!grabbing)
            {
                grabbing = true;
                if(currentInteractable && currentInteractable.IsGrabbable() && !currentInteractable.IsHeld())
                {
                    grabPoint.position = currentInteractable.transform.position;
                    currentInteractable.Grab(this);
                    grabbedInteractable = currentInteractable;
                }
            }
        } else
        {
            if(grabbing)
            {
                grabbing = false;
                if(grabbedInteractable)
                {
                    grabbedInteractable.Drop(this);
                    grabbedInteractable = null;
                }
            }
        }
    }

    void Update()
    {

        if(grabbedInteractable)
        {
            l.SetPositions(new Vector3[] { aimRef.position, grabbedInteractable.transform.position });
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(new Ray(aimRef.position, aimRef.forward), out hit, Mathf.Infinity, DBXRResources.Main.InteractLayerMask))
            {
                l.SetPositions(new Vector3[] { aimRef.position, hit.point });
                if (currentInteractable == null)
                {
                    currentInteractable = hit.transform.GetComponent<InteractableComponent>();
                    currentInteractable.RayEntered.Invoke(this);
                } else
                {
                    if(currentInteractable.transform != hit.transform)
                    {
                        currentInteractable.RayExited.Invoke(this);
                        currentInteractable = hit.transform.GetComponent<InteractableComponent>();
                        currentInteractable.RayEntered.Invoke(this);
                    }
                }
            }
            else
            {
                l.SetPositions(new Vector3[] { aimRef.position, aimRef.position + (aimRef.forward * 100) });
                if(currentInteractable)
                {
                    currentInteractable.RayExited.Invoke(this);
                }
                currentInteractable = null;
            }
        }

    }

}
