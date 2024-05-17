using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SystemType
{
    Battery,
    Oxygen,
    Comms,
    Fan,
    Pump,
    CO2,
    CommTower,
}

public class MIKESystemManager : MonoBehaviour
{
    public static MIKESystemManager Main { get; private set; }
    public Dictionary<SystemType, SystemStatus> SystemStatuses { get; private set; }


    void Awake()
    {
        if (Main == null)
            Main = this;
        else
            Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        InitSystemStatuses();
        TSSManager.Main.OnDCUUpdated += UpdateDCU;
        TSSManager.Main.OnCommUpdated += UpdateComm;
    }

    private void InitSystemStatuses()
    {
        SystemStatuses = new Dictionary<SystemType, SystemStatus>
        {
            { SystemType.Battery, new SystemStatus("Suit Battery", "Umbilical Power") },
            { SystemType.Oxygen, new SystemStatus("Primary Tank", "Secondary Tank") },
            { SystemType.Comms, new SystemStatus("Channel A", "Channel B") },
            { SystemType.Fan, new SystemStatus("Primary Fan", "Secondary Fan") },
            { SystemType.Pump, new SystemStatus("Open", "Closed") },
            { SystemType.CO2, new SystemStatus("Scrubber A", "Scrubber B") },
            { SystemType.CommTower, new SystemStatus("On", "Off") },
        };
    }

    private void UpdateDCU(DCUData data)
    {
        CheckForDifferentDCU(data);

        SystemStatuses[SystemType.Battery].Value = data.batt;
        SystemStatuses[SystemType.Oxygen].Value = data.oxy;
        SystemStatuses[SystemType.Comms].Value = data.comm;
        SystemStatuses[SystemType.Fan].Value = data.fan;
        SystemStatuses[SystemType.Pump].Value = data.pump;
        SystemStatuses[SystemType.CO2].Value = data.co2;
    }

    private void UpdateComm(CommData data)
    {
        CheckForDifferentComm(data);

        SystemStatuses[SystemType.CommTower].Value = data.comm_tower;
    }

    private void CheckForDifferentDCU(DCUData data)
    {
        if (data.batt != SystemStatuses[SystemType.Battery].Value)
        {
            MIKENotificationManager.Main.SendNotification("NOTIFICATION", "Battery switched to " + SystemStatuses[SystemType.Battery].GetInactiveStatus().ToLower(), MIKEResources.Main.WarningNotificationColor, 5f);
        }

        if (data.oxy != SystemStatuses[SystemType.Oxygen].Value)
        {
            MIKENotificationManager.Main.SendNotification("NOTIFICATION", "Oxygen tank switched to " + SystemStatuses[SystemType.Oxygen].GetInactiveStatus().ToLower(), MIKEResources.Main.WarningNotificationColor, 5f);
        }

        if (data.comm != SystemStatuses[SystemType.Comms].Value)
        {
            MIKENotificationManager.Main.SendNotification("NOTIFICATION", "Comms switched to " + SystemStatuses[SystemType.Comms].GetInactiveStatus(), MIKEResources.Main.WarningNotificationColor, 5f);
        }

        if (data.fan != SystemStatuses[SystemType.Fan].Value)
        {
            MIKENotificationManager.Main.SendNotification("NOTIFICATION", "Fan switched to " + SystemStatuses[SystemType.Fan].GetInactiveStatus().ToLower(), MIKEResources.Main.WarningNotificationColor, 5f);
        }

        if (data.pump != SystemStatuses[SystemType.Pump].Value)
        {
            MIKENotificationManager.Main.SendNotification("NOTIFICATION", "Pump switched to " + SystemStatuses[SystemType.Pump].GetInactiveStatus().ToLower(), MIKEResources.Main.WarningNotificationColor, 5f);
        }

        if (data.co2 != SystemStatuses[SystemType.CO2].Value)
        {
            MIKENotificationManager.Main.SendNotification("NOTIFICATION", "CO2 scrubber switched to " + SystemStatuses[SystemType.CO2].GetInactiveStatus().ToLower(), MIKEResources.Main.WarningNotificationColor, 5f);
        }
    }

    private void CheckForDifferentComm(CommData data)
    {
        if (data.comm_tower != SystemStatuses[SystemType.CommTower].Value)
        {
            MIKENotificationManager.Main.SendNotification("NOTIFICATION", "Comm tower switched to " + SystemStatuses[SystemType.CommTower].GetInactiveStatus(), MIKEResources.Main.WarningNotificationColor, 5f);
        }
    }
}

public class SystemStatus
{
    public bool Value { get; set; }
    private string ifTrue;
    private string ifFalse;

    public SystemStatus(string ifTrue, string ifFalse)
    {
        this.ifTrue = ifTrue;
        this.ifFalse = ifFalse;
    }

    public string GetActiveStatus()
    {
        return Value ? ifTrue : ifFalse;
    }

    public string GetInactiveStatus()
    {
        return Value ? ifFalse : ifTrue;
    }
}
