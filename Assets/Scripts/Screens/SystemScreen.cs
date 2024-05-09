using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemScreen : LMCCScreen
{
    [Header("DCU Values")]
    [SerializeField] private MIKEWidgetValue battery;
    [SerializeField] private MIKEWidgetValue oxygen;
    [SerializeField] private MIKEWidgetValue comms;
    [SerializeField] private MIKEWidgetValue fan;
    [SerializeField] private MIKEWidgetValue pump;
    [SerializeField] private MIKEWidgetValue co2;

    [Header("Comm Values")]
    [SerializeField] private MIKEWidgetValue commTower;

    // Start is called before the first frame update
    void Start()
    {
        TSSManager.Main.OnDCUUpdated += UpdateDCU;
        TSSManager.Main.OnCommUpdated += UpdateComm;
    }

    void OnEnable()
    {
        //if (TSSManager.Main.DCUData != null)
        UpdateDCU(TSSManager.Main.DCUData);
        //if (TSSManager.Main.CommData != null)
        UpdateComm(TSSManager.Main.CommData);
    }

    private void UpdateDCU(DCUData data)
    {
        Debug.Log("UpdateDCU");
        battery.SetValue(data.batt ? "Suit Battery" : "Umbilical Power", data.batt ? MIKEResources.Main.WarningNotificationColor : MIKEResources.Main.PositiveNotificationColor);
        oxygen.SetValue(data.oxy ? "Primary" : "Secondary", data.oxy ? MIKEResources.Main.PositiveNotificationColor : MIKEResources.Main.WarningNotificationColor);
        comms.SetValue(data.comm ? "A" : "B", data.comm ? MIKEResources.Main.PositiveNotificationColor : MIKEResources.Main.WarningNotificationColor);
        fan.SetValue(data.fan ? "Primary" : "Secondary", data.fan ? MIKEResources.Main.PositiveNotificationColor : MIKEResources.Main.WarningNotificationColor);
        pump.SetValue(data.pump ? "Open" : "Closed", data.pump ? MIKEResources.Main.PositiveNotificationColor : MIKEResources.Main.WarningNotificationColor);
        co2.SetValue(data.co2 ? "A" : "B", data.co2 ? MIKEResources.Main.PositiveNotificationColor : MIKEResources.Main.WarningNotificationColor);
    }

    private void UpdateComm(CommData data)
    {
        commTower.SetValue(data.comm_tower ? "Powered On" : "Powered Off", data.comm_tower ? MIKEResources.Main.PositiveNotificationColor : MIKEResources.Main.WarningNotificationColor);
    }
}
