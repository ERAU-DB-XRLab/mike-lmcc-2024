using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeverComponent : InteractableComponent
{

    [SerializeField] private Transform clampPos, leverHandle;
    [SerializeField] private float minRotation, maxRotation;
    private Transform currentHand;
    private float currentZ;

    public UnityEvent<float> GrabUpdate;

    [Space]
    [SerializeField] private bool boundLever;
    [SerializeField] private float returnSpeed, returnRotation;

    // Start is called before the first frame update
    new void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        if (currentHand)
        {

            // Calculate direction

            // The hand's parent's parent is the object in Unity that is actually tracked
            Transform handParent = currentHand.parent;
            Vector3 handDir = (handParent.position - leverHandle.position).normalized;
            Vector3 normal = Vector3.Cross(leverHandle.forward, leverHandle.up).normalized;

            Vector3 toSubtract = Vector3.Dot(handDir, normal) * normal;
            Vector3 actualLeverDir = handDir - toSubtract;

            leverHandle.forward = actualLeverDir;

            GrabUpdate.Invoke(currentZ);

        }

        // Clamp rotation
        float localRotation = leverHandle.localEulerAngles.x;
        if (localRotation > 180)
        {
            localRotation -= 360;
        }

        localRotation = Mathf.Clamp(localRotation, minRotation, maxRotation);

        if (!currentHand && boundLever)
        {
            localRotation = Mathf.MoveTowards(localRotation, returnRotation, returnSpeed * Time.deltaTime);
        }

        // store value
        currentZ = localRotation;

        leverHandle.localRotation = Quaternion.Euler(localRotation, 0, 0);

    }

    public override void Grab(HandInteract interact)
    {
        currentHand = interact.GetHandTransform();
        interact.SetHandOverride(clampPos);
    }

    public override void Drop(HandInteract interact)
    {
        currentHand = null;
        interact.SetHandOverride(null);
    }

    public float GetValue()
    {
        return (currentZ - minRotation) / (Mathf.Abs(minRotation) + Mathf.Abs(maxRotation));
    }

    public void SetRotation(float rot)
    {
        leverHandle.localRotation = Quaternion.Euler(rot, 0, 0);
    }

    public float GetMaxRotation()
    {
        return maxRotation;
    }

}