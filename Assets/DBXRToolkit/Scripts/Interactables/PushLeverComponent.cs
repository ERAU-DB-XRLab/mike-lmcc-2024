using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PushLeverComponent : InteractableComponent
{

    public UnityEvent<float> GrabUpdate;

    [SerializeField] private Transform leverHandle, clampPos;

    [Space] // Local position of the "lever" object when it is fully extended and fully pressed
    [SerializeField] private float outValue;
    [SerializeField] private float inValue;
    private Transform currentHand;
    private float currentValue;

    // Start is called before the first frame update
    new void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        if (currentHand)
        {

            Vector3 localPos = transform.InverseTransformPoint(currentHand.parent.position);
            float value = localPos.x;
            currentValue = Mathf.Clamp(value, outValue, inValue);

            leverHandle.localPosition = new Vector3(currentValue, 0, 0);

            GrabUpdate.Invoke(currentValue);

        }

    }

    public override void Grab(HandInteract interact)
    {
        currentHand = interact.GetHandTransform();
        if (interact is HandControllerInteract)
        {
            ((HandControllerInteract)interact).SetHandOverride(clampPos);
        }
    }

    public override void Drop(HandInteract interact)
    {
        currentHand = null;
        if (interact is HandControllerInteract)
        {
            ((HandControllerInteract)interact).SetHandOverride(null);
        }
    }

    public float GetValue()
    {
        return (currentValue - outValue) / (inValue - outValue);
    }
}
