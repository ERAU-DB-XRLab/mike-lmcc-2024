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
    [SerializeField] protected bool toggle;
    [SerializeField] private float pressCooldown = 0.25f;
    private Vector3 unpressedPos, pressedPos, desiredPos;
    private Vector3 refVel;
    private int pointerCount = 0;
    protected bool pressed = false;
    //private bool reset = false;

    protected bool onButtonCooldown = false;
    protected bool onButtonExit = false;
    private float timer = 0f;

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

        if (onButtonCooldown && onButtonExit)
        {
            timer += Time.deltaTime;
            if (timer >= pressCooldown)
            {
                onButtonCooldown = false;
                onButtonExit = false;
                timer = 0f;
            }
        }
    }

    public virtual void PointerEnter(HandInteract interact)
    {
        if (onButtonCooldown)
            return;
        ChangePointerCount(1);
        onButtonCooldown = true;
    }

    public virtual void PointerExit(HandInteract interact)
    {
        if (onButtonExit)
            return;
        if (onButtonCooldown)
            onButtonExit = true;
        ChangePointerCount(-1);
    }

    public void ChangePointerCount(int change)
    {

        pointerCount = Mathf.Clamp(pointerCount + change, 0, 1);

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
            if (pointerCount > 0)// && reset)
            {

                //reset = false;

                if (pressed)
                {
                    desiredPos = unpressedPos;
                }
                else
                {
                    desiredPos = pressedPos;
                }

                pressed = !pressed;
                ValueChanged.Invoke(pressed);
            }
        }

        /*if (pointerCount == 0)
        {
            reset = true;
        }*/

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
