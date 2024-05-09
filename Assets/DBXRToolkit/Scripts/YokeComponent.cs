using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class YokeComponent : InteractableComponent
{

    public UnityEvent<Vector2> GrabUpdate;

    [SerializeField] private Transform yoke, clampPosLeft, clampPosRight;

    [Space] // Local position of the "lever" object when it is fully extended and fully pressed
    [SerializeField] private float outValue;
    [SerializeField] private float inValue;
    private HandInteract primaryHand, secondaryHand;
    private Vector2 currentValue;

    private Transform primaryClampPos, secondaryClampPos;
    private Quaternion desiredRot;

    // Start is called before the first frame update
    new void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        if (primaryHand)
        {

            Vector3 localPos = transform.InverseTransformPoint(primaryHand.GetHandTransform().parent.position);
            float value = localPos.z;
            currentValue.x = Mathf.Clamp(value, outValue, inValue);

            yoke.localPosition = new Vector3(0, 0, currentValue.x);


            if(!secondaryHand)
            {
                desiredRot = Quaternion.LookRotation(transform.forward, primaryHand.transform.forward);
            } else
            {
                float primaryY = transform.InverseTransformPoint(primaryHand.transform.position).y;
                float secondaryY = transform.InverseTransformPoint(secondaryHand.transform.position).y;
                desiredRot = Quaternion.LookRotation(transform.forward, primaryY > secondaryY ? primaryHand.transform.forward : secondaryHand.transform.forward);
            }

            yoke.transform.rotation = Quaternion.RotateTowards(yoke.transform.rotation, desiredRot, 500f * Time.deltaTime);
            currentValue.y = yoke.transform.localEulerAngles.z;
            if(currentValue.y > 180)
            {
                currentValue.y -= 360;
            }

            GrabUpdate.Invoke(GetValue());

        }

    }

    public override void Grab(HandInteract interact)
    {

        if(primaryHand == null)
        {
            primaryHand = interact;
            float leftDist = Vector3.SqrMagnitude(clampPosLeft.position - interact.GetHandTransform().position);
            float rightDist = Vector3.SqrMagnitude(clampPosRight.position - interact.GetHandTransform().position);
            if(leftDist < rightDist)
            {
                interact.SetHandOverride(clampPosLeft);
                primaryClampPos = clampPosLeft;
            } else 
            {
                interact.SetHandOverride(clampPosRight);
                primaryClampPos = clampPosRight;
            }
        } else 
        {
            secondaryHand = interact;
            if(primaryClampPos == clampPosLeft)
            {
                interact.SetHandOverride(clampPosRight);
                secondaryClampPos = clampPosRight;
            } else 
            {
                interact.SetHandOverride(clampPosLeft);
                secondaryClampPos = clampPosLeft;
            }
        }

    }

    public override void Drop(HandInteract interact)
    {
        if(secondaryHand == null)
        {
            primaryHand = null;
            primaryClampPos = null;
            interact.SetHandOverride(null);
        } else 
        {
            if(interact == primaryHand)
            {
                primaryHand.SetHandOverride(null);
                primaryHand = secondaryHand;
                primaryClampPos = secondaryClampPos;
                primaryHand.SetHandOverride(primaryClampPos);

                secondaryHand = null;
                secondaryClampPos = null;

            } else 
            {
                secondaryHand.SetHandOverride(null);
                secondaryHand = null;
                secondaryClampPos = null;
            }
        }
    }

    public Vector2 GetValue()
    {
        float translationValue = (currentValue.x - outValue) / (inValue - outValue);
        float rotationValue = currentValue.y;
        return new Vector2(translationValue, rotationValue);
    }
}