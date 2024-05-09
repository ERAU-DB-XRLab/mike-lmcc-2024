using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandInteract : MonoBehaviour
{

    [SerializeField] private InputActionProperty grab, interact;
    [SerializeField] private HandAnimator anim;
    [SerializeField] private Transform grabPoint;
    public Transform player;

    private Velocity velComponent;
    private InteractableComponent currentInteractable;

    // The hand transform
    private Transform handTransform;

    // If this transform is not null, the player's hand model will snap to it and stay there
    private Transform handOverride;

    // Default hand positions
    private Vector3 defaultPos;
    private Quaternion defaultRot;

    void Awake()
    {
        velComponent = GetComponent<Velocity>();
        handTransform = anim.transform;
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
                currentInteractable.Grab(this);
            }
        }

        if (grab.action.WasReleasedThisFrame())
        {
            if (currentInteractable)
            {
                currentInteractable.Drop(this);
                currentInteractable = null;
                anim.SetFistOverride(false);
            }
        }

        if (interact.action.WasPressedThisFrame())
        {
            if (currentInteractable)
            {
                currentInteractable.InteractStart(this);
            }
        }

        if (interact.action.WasReleasedThisFrame())
        {
            if (currentInteractable)
            {
                currentInteractable.InteractStop(this);
            }
        }

        if (handOverride)
        {
            ClampToOverride();
        }

    }

    public Transform GetHandTransform()
    {
        return handTransform;
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
                ic.HandEntered(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == DBXRResources.Main.InteractLayer)
        {
            InteractableComponent ic = other.GetComponent<InteractableComponent>();
            if (ic)
                ic.HandExited(this);
        }
    }

    public Vector3 GetVelocity()
    {
        return velComponent.GetVelocity();
    }

}
