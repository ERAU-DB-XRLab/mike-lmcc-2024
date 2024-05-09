using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimator : MonoBehaviour
{

    [SerializeField] private Animator anim;
    [SerializeField] private InputActionProperty trigger, grip;

    private bool fistOverride;

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("GripDown", grip.action.IsPressed());
        anim.SetBool("TriggerDown", trigger.action.IsPressed());
        anim.SetBool("FistOverride", fistOverride);
    }

    public void SetFistOverride(bool b)
    {
        this.fistOverride = b;
    }

}
