using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScreenType
{
    Vitals,
    Rover,
    Message,
    System,
    Astronaut,
    Science,
    Procedure,
    Settings,
}

public abstract class LMCCScreen : MonoBehaviour
{
    public ScreenType ScreenType { get { return screenType; } }
    [SerializeField] private ScreenType screenType;

    public virtual void ScreenActivated() { }
    public virtual void ScreenDeactivated() { }
}
