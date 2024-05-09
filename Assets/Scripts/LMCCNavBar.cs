using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMCCNavBar : MonoBehaviour
{
    [SerializeField] private List<ButtonComponent> buttons;

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
            //LMCCMenuSpawner.Main.SpawnMenu(index);
        }
    }
}
