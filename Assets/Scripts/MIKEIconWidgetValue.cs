using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MIKEIconWidgetValue : MIKEWidgetValue
{
    [SerializeField] private string rounding = "0";
    [SerializeField] private float maxValue;
    [SerializeField] private float minValue;
    [Space]
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text valueText;

    private string initialColorHex;

    protected override void InitText()
    {
        initialColorHex = ColorUtility.ToHtmlStringRGB(valueText.color);
    }

    public void SetFill(float value)
    {
        fillImage.fillAmount = (value - minValue) / (maxValue - minValue);
    }

    public override void SetValue(float value)
    {
        valueText.SetText("<color=#" + (value < minValue || value > maxValue ? ColorUtility.ToHtmlStringRGB(MIKEResources.Main.NegativeNotificationColor) : initialColorHex) + ">" + value.ToString(rounding) + " " + units);
        SetFill(value);
    }
}
