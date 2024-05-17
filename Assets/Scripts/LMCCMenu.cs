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

    public bool IsActive
    {
        get { return gameObject.activeSelf; }
    }

    [SerializeField] private TMP_Text titleText;
    [SerializeField] private LMCCScreen currentScreen;

    private MenuComponent currentComponent;

    protected override void Awake()
    {
        base.Awake();
        currentComponent = GetComponent<MenuComponent>();
    }

    // Start is called before the first frame update
    void Start()
    {
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
        component.InvertGrabPoint(false);
    }

    public void DroppedMenu(MenuComponent component, HandInteract interact)
    {
        LMCCMenuSpawner.Main.DisplayMenuBoxes(false);
    }
}