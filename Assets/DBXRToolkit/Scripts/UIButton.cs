using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIButton : MonoBehaviour
{

    [SerializeField] private Button button;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private bool toggle;
    [Space]
    [Header("Color Info")]
    [SerializeField] private Color normalColor;
    [SerializeField] private Color selectedColor;

    private bool pressed;

    public void Press()
    {
        buttonText.color = selectedColor;
        if(!toggle)
        {
            pressed = true;
        } else
        {
            pressed = !pressed;
        }
        UpdateColor();

        if(pressed)
            button.onClick.Invoke();

    }

    public void Release()
    {
        if(!toggle)
        {
            pressed = false;
            UpdateColor();
        }
    }

    public void UpdateColor()
    {
        buttonText.color = pressed ? selectedColor : normalColor;
    }

}