using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class LMCCMenuBox : MonoBehaviour
{
    private const float menuPositionOffset = 400f;

    private LMCCMenu currentMenu;
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    void OnTriggerEnter(Collider other)
    {
        LMCCMenu menu = other.gameObject.GetComponent<LMCCMenu>();
        if (menu != null && currentMenu == null)
        {
            Debug.Log(other.name + " entered");

            menu.GetComponent<MenuComponent>().OnUIDropped += FillMenuBox;
            image.CrossFadeAlpha(1.5f, 0.1f, false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        LMCCMenu menu = other.gameObject.GetComponent<LMCCMenu>();

        if (menu != null)
        {
            menu.GetComponent<MenuComponent>().OnUIDropped -= FillMenuBox;

            if (currentMenu == null)
            {
                Debug.Log(other.name + " exited");
            }
            else if (menu == currentMenu)
            {
                currentMenu = null;
            }

            image.CrossFadeAlpha(1f, 0.1f, false);
        }
    }

    private void FillMenuBox(MenuComponent component, HandInteract interact)
    {
        if (currentMenu == null)
        {
            Debug.Log("Filling menu box");
            currentMenu = component.GetComponent<LMCCMenu>();
            currentMenu.transform.SetParent(this.transform);
            currentMenu.transform.SetLocalPositionAndRotation(Vector3.up * menuPositionOffset, Quaternion.identity);
            image.CrossFadeAlpha(0f, 0.1f, false);
        }
    }
}
