using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMCCMenu : FadeBehavior
{
    public LMCCScreen CurrentScreen
    {
        get { return currentScreen; }
        set { currentScreen = value; }
    }

    [SerializeField] private LMCCScreen currentScreen;
}