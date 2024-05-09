using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonComponent : InteractableComponent
{

    public UnityEvent<bool> ValueChanged;

    [SerializeField] private InteractionDirection pressDirection;
    [SerializeField] private Transform buttonObj;
    [SerializeField] private float pressAmount;
    [Space]
    [SerializeField] private bool toggle;
    private Vector3 unpressedPos, pressedPos, desiredPos;
    private Vector3 refVel;
    private int pointerCount = 0;
    private bool pressed = false;
    private bool reset = false;

    new void Awake()
    {
        base.Awake();
        unpressedPos = buttonObj.localPosition;
        pressedPos = buttonObj.localPosition + (GetPressVector() * pressAmount);
        desiredPos = unpressedPos;

        PointerEntered.AddListener(PointerEnter);
        PointerExited.AddListener(PointerExit);

    }

    void Update()
    {
        buttonObj.localPosition = Vector3.SmoothDamp(buttonObj.localPosition, desiredPos, ref refVel, 0.025f);
    }

    public virtual void PointerEnter(HandInteract interact)
    {
        ChangePointerCount(1);
    }

    public virtual void PointerExit(HandInteract interact)
    {
        ChangePointerCount(-1);
    }

    public void ChangePointerCount(int change)
    {

        pointerCount += change;

        if (!toggle)
        {
            if (pressed && pointerCount == 0)
            {
                pressed = false;
                desiredPos = unpressedPos;
                ValueChanged.Invoke(pressed);
            }
            else
            if (!pressed && pointerCount > 0)
            {
                pressed = true;
                desiredPos = pressedPos;
                ValueChanged.Invoke(pressed);
            }
        }
        else
        {
            if (pointerCount > 0 && reset)
            {

                reset = false;
                pressed = !pressed;

                if (pressed)
                {
                    desiredPos = pressedPos;
                }
                else
                {
                    desiredPos = unpressedPos;
                }

                ValueChanged.Invoke(pressed);

            }
        }

        if (pointerCount == 0)
        {
            reset = true;
        }

    }

    public Vector3 GetPressVector()
    {

        return DBXRResources.Main.GetDirectionFromTransform(buttonObj, pressDirection);

    }

    public bool IsPressed()
    {
        return pressed;
    }

}
