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
        value.SetValue(TimeSpan.FromSeconds(TSSManager.Main.EVATime).Hours + ":" + TimeSpan.FromSeconds(TSSManager.Main.EVATime).Minutes + ":" + TimeSpan.FromSeconds(TSSManager.Main.EVATime).Seconds, MIKEResources.Main.PositiveNotificationColor);
    }
}
