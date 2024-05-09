using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerInteract : MonoBehaviour
{

    private HandInteract interact;

    void Awake()
    {
        interact = GetComponentInParent<HandInteract>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == DBXRResources.Main.InteractLayer)
        {
            InteractableComponent ic = other.gameObject.GetComponent<InteractableComponent>();
            if(ic)
            {
                ic.PointerEntered(interact);
            }
        } else
        if(other.gameObject.layer == DBXRResources.Main.UILayer)
        {
            UIButton button = other.gameObject.GetComponent<UIButton>();
            if(button)
            {
                button.Press();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == DBXRResources.Main.InteractLayer)
        {
            InteractableComponent ic = other.gameObject.GetComponent<InteractableComponent>();
            if(ic)
            {
                ic.PointerExited(interact);
            }
        } else
        if (other.gameObject.layer == DBXRResources.Main.UILayer)
        {
            UIButton button = other.gameObject.GetComponent<UIButton>();
            if (button)
            {
                button.Release();
            }
        }
    }

}
