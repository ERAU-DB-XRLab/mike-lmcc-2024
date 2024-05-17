using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MIKEVitalsWidgetValue : MIKEWidgetValue
{
    [SerializeField] private string rounding = "0.0";

    private float currValue;
    private Color currColor;
    private string colorHex;
    private float? nomimalValue;
    private float maxValue;
    private float minValue;

    public float CurrentValue { get { return currValue; } }
    public float MaxValue { get { return maxValue; } }
    public float MinValue { get { return minValue; } }
    public float? NominalValue { get { return nomimalValue; } }

    public void SetBounds(float min, float max, float? nominal = null)
    {
        nomimalValue = nominal;
        maxValue = max;
        minValue = min;
    }

    public override void SetValue(float value)
    {
        if (text == null)
            InitText();

        currValue = value;
        CalculateColor();
        colorHex = ColorUtility.ToHtmlStringRGB(currColor);
        text.SetText(fieldText + " <color=#" + colorHex + ">" + currValue.ToString(rounding) + " " + units);
    }

    private void CalculateColor()
    {
        if (currValue > maxValue || currValue < minValue)
        {
            currColor = MIKEResources.Main.NegativeNotificationColor;
        }
        else if (!nomimalValue.HasValue)
        {
            currColor = MIKEResources.Main.PositiveNotificationColor;
        }
        else if ((nomimalValue == minValue || nomimalValue == maxValue) && currValue == nomimalValue)
        {
            currColor = MIKEResources.Main.PositiveNotificationColor;
        }
        else
        {
            float minLerp = (float)nomimalValue;
            float maxLerp;

            if (currValue >= nomimalValue)
            {
                maxLerp = maxValue;
            }
            else if (currValue < nomimalValue)
            {
                maxLerp = minValue;
            }
            else
            {
                Debug.LogError("Error in MIKEWidgetValue.cs: The current value is not greater than or less than the nomimal value");
                return;
            }

            currColor = Color.Lerp(MIKEResources.Main.WarningNotificationColor, MIKEResources.Main.PositiveNotificationColor, Mathf.InverseLerp(maxLerp, minLerp, currValue));
        }
    }
}
