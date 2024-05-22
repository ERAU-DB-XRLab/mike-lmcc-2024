using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIKETime : MonoBehaviour
{
    private MIKEWidgetValue value;

    // Start is called before the first frame update
    void Start()
    {
        value = GetComponentInChildren<MIKEWidgetValue>();
    }

    // Update is called once per frame
    void Update()
    {
        value.SetValue(TimeSpan.FromSeconds(TSSManager.Main.EVATime).Hours.ToString("00") + ":" + TimeSpan.FromSeconds(TSSManager.Main.EVATime).Minutes.ToString("00") + ":" + TimeSpan.FromSeconds(TSSManager.Main.EVATime).Seconds.ToString("00"), MIKEResources.Main.PositiveNotificationColor);
    }
}
