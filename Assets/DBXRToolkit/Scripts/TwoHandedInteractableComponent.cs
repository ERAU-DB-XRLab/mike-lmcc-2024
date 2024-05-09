using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoHandedInteractableComponent : InteractableComponent
{

    [SerializeField] private InteractionDirection interactableDirection;
    [SerializeField] private Transform secondHandClampPos;

    private HandInteract primary, secondary;

    public override void Grab(HandInteract interact)
    {
        if(primary == null)
        {
            primary = interact;
            base.Grab(interact);
        } else
        {
            secondary = interact;
            
        }
    }

    public override void Drop(HandInteract interact)
    {
        if(interact == primary)
        {
            primary = null;
            base.Drop(interact);
        } else
        {
            secondary = null;
            transform.localPosition = pos;
            transform.localRotation = Quaternion.Euler(rot);
        }
    }

    void Update()
    {
        if(primary && secondary)
        {
            Vector3 dir = secondary.transform.position - primary.transform.position;
            transform.position = primary.transform.position;

            switch (interactableDirection)
            {

                case InteractionDirection.POSITIVE_X:
                    transform.right = dir;
                    break;

                case InteractionDirection.POSITIVE_Y:
                    transform.up = dir;
                    break;

                case InteractionDirection.POSITIVE_Z:
                    transform.forward = dir;
                    break;

                case InteractionDirection.NEGATIVE_X:
                    transform.right = -dir;
                    break;

                case InteractionDirection.NEGATIVE_Y:
                    transform.up = -dir;
                    break;

                case InteractionDirection.NEGATIVE_Z:
                    transform.forward = -dir;
                    break;

            }

        }
    }

}
