using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MIKEWidgetValue : MonoBehaviour
{
    [SerializeField] protected string units;

    protected TextMeshProUGUI text;
    protected string fieldText;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        InitText();
    }

    protected virtual void InitText()
    {
        text = GetComponent<TextMeshProUGUI>();
        fieldText = text.text;
    }

    public virtual void SetValue(float value)
    {
        text.SetText(fieldText + " " + value.ToString() + " " + units);
    }

    public virtual void SetValue(float value, Color color)
    {
        text.SetText(fieldText + " <color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + value.ToString() + " " + units);
    }

    public virtual void SetValue(string value)
    {
        text.SetText(fieldText + " " + value);
    }

    public virtual void SetValue(string value, Color color)
    {
        text.SetText(fieldText + " <color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + value);
    }

    public virtual void SetValue(bool value)
    {
        string colorHex = ColorUtility.ToHtmlStringRGB(value ? MIKEResources.Main.PositiveNotificationColor : MIKEResources.Main.NegativeNotificationColor);
        text.SetText(fieldText + " <color=#" + ColorUtility.ToHtmlStringRGB(value ? MIKEResources.Main.PositiveNotificationColor : MIKEResources.Main.NegativeNotificationColor) + ">" + value.ToString());
    }
}
