using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class MIKEIconWidgetValue : MIKEWidgetValue
{
    [SerializeField] private Image iconImage;

    [SerializeField] private float maxValue;
    [SerializeField] private float minValue;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetFill(float value)
    {
        iconImage.fillAmount = (value - minValue) / (maxValue - minValue);
    }
}
