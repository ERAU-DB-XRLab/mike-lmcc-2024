using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LMCCMenu : LMCCFadeBehavior
{
    public LMCCScreen CurrentScreen
    {
        get { return currentScreen; }
        set { currentScreen = value; }
    }

    private TMP_Text titleText;
    private MenuComponent currentComponent;

    [SerializeField] private LMCCScreen currentScreen;

    // Start is called before the first frame update
    void Start()
    {
        currentComponent = GetComponent<MenuComponent>();
        titleText = GetComponentInChildren<TMP_Text>();

        currentComponent.OnUIGrabbed += GrabbedMenu;
        currentComponent.OnUIDropped += DroppedMenu;
    }

    public void SetTitle(string title)
    {
        titleText.text = title;
    }

    public void GrabbedMenu(MenuComponent component, HandInteract interact)
    {
        LMCCMenuSpawner.Main.DisplayMenuBoxes(true);
    }

    public void DroppedMenu(MenuComponent component, HandInteract interact)
    {
        LMCCMenuSpawner.Main.DisplayMenuBoxes(false);
    }
}