using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlightStickComponent : InteractableComponent
{

    [SerializeField] private Transform clampPos, handle;
    [SerializeField] private float minRotation, maxRotation;
    private Transform currentHand;
    private Vector2 currentRotation;

    public UnityEvent<Vector2> GrabUpdate;

    [Space]
    [SerializeField] private bool bound;
    [SerializeField] private float returnSpeed;

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
            Vector3 handDir = (handParent.position - handle.position).normalized;
            handle.up = handDir;

            GrabUpdate.Invoke(currentRotation);

        }

        // Clamp rotation
        float localRotationX = handle.localEulerAngles.x;
        float localRotationZ = handle.localEulerAngles.z;
        if (localRotationX > 180)
        {
            localRotationX -= 360;
        }
        if (localRotationZ > 180)
        {
            localRotationZ -= 360;
        }

        localRotationX = Mathf.Clamp(localRotationX, minRotation, maxRotation);
        localRotationZ = Mathf.Clamp(localRotationZ, minRotation, maxRotation);

        if (!currentHand && bound)
        {
            localRotationX = Mathf.MoveTowards(localRotationX, 0, returnSpeed * Time.deltaTime);
            localRotationZ = Mathf.MoveTowards(localRotationZ, 0, returnSpeed * Time.deltaTime);
        }

        // store value
        currentRotation = new Vector2(localRotationX, localRotationZ);

        handle.localRotation = Quaternion.Euler(localRotationX, 0, localRotationZ);

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

    public Vector2 GetValue()
    {

        float currentX = (currentRotation.x - minRotation) / (Mathf.Abs(minRotation) + Mathf.Abs(maxRotation));
        float currentZ = (currentRotation.y - minRotation) / (Mathf.Abs(minRotation) + Mathf.Abs(maxRotation));

        return new Vector2(currentX, currentZ);

    }

    public void SetRotation(Vector2 rot)
    {
        handle.localRotation = Quaternion.Euler(rot.x, 0, rot.y);
    }

    public float GetMaxRotation()
    {
        return maxRotation;
    }

}