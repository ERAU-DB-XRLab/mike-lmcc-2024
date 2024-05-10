using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMCCNavBar : LMCCFadeBehavior
{
    public static LMCCNavBar Main { get; private set; }

    public List<ButtonComponent> Buttons { get { return buttons; } }

    [SerializeField] private List<ButtonComponent> buttons;

    protected override void Awake()
    {
        base.Awake();
        if (Main == null)
            Main = this;
        else
            Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;
            buttons[i].ValueChanged.AddListener((pressed) => ButtonClicked(pressed, index));
        }
    }

    public void ButtonClicked(bool pressed, int index)
    {
        if (pressed)
        {
            LMCCMenuSpawner.Main.ToggleMenu(index);
        }
    }
}
