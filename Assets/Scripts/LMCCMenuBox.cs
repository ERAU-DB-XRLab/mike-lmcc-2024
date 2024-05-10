using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class LMCCMenuBox : MonoBehaviour
{
    [SerializeField] private bool invertGrabPoint = false;
    private const float menuPositionOffset = 400f;

    public Image Image { get { return image; } }
    public LMCCMenu CurrentMenu { get { return currentMenu; } set { currentMenu = value; } }

    [Header("For Debugging, keep this null")]
    [SerializeField] private LMCCMenu currentMenu;
    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void OnTriggerEnter(Collider other)
    {
        LMCCMenu menu = other.gameObject.GetComponent<LMCCMenu>();
        if (menu != null && currentMenu == null)
        {
            AddFillEvent(menu.GetComponent<MenuComponent>());
            image.CrossFadeAlpha(2f, 0.1f, false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        LMCCMenu menu = other.gameObject.GetComponent<LMCCMenu>();

        if (menu != null)
        {
            RemoveFillEvent(menu.GetComponent<MenuComponent>());

            if (menu == currentMenu)
            {
                currentMenu = null;
            }

            if (currentMenu == null)
            {
                image.CrossFadeAlpha(1f, 0.1f, false);
            }
        }
    }

    public void AddFillEvent(MenuComponent menu)
    {
        menu.OnUIDropped += FillMenuBox;
    }

    public void RemoveFillEvent(MenuComponent menu)
    {
        menu.OnUIDropped -= FillMenuBox;
    }

    private void FillMenuBox(MenuComponent component, HandInteract interact)
    {
        if (currentMenu == null)
        {
            currentMenu = component.GetComponent<LMCCMenu>();
            currentMenu.transform.SetParent(this.transform);

            if (invertGrabPoint)
            {
                currentMenu.transform.SetLocalPositionAndRotation(Vector3.down * menuPositionOffset, Quaternion.identity);
                component.InvertGrabPoint(true);
            }
            else
            {
                currentMenu.transform.SetLocalPositionAndRotation(Vector3.up * menuPositionOffset, Quaternion.identity);
                component.InvertGrabPoint(false);
            }

            image.CrossFadeAlpha(0f, 0.1f, false);
        }
    }
}
