using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AlertType
{
    HeartRate,
    O2SuitPressure,
    CO2SuitPressure,
    OtherSuitPressure,
    TotalSuitPressure,
    CO2HelmetPressure,
    PrimaryFanRPM,
    SecondaryFanRPM,
    CO2ScrubberAStorage,
    CO2ScrubberBStorage,
    Temperature
}

public class MIKEAlertManager : MonoBehaviour
{
    public static MIKEAlertManager Main { get; private set; }
    public Dictionary<AlertType, Alert> Alerts { get; private set; }

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
        InitAlerts();
        TSSManager.Main.OnTelemetryUpdated += CheckForAlerts;
    }

    private void InitAlerts()
    {
        Alerts = new Dictionary<AlertType, Alert>
        {
            { AlertType.HeartRate, new Alert(50, 160, "Heart rate is too low!", "Heart rate is too high!") },
            { AlertType.O2SuitPressure, new Alert(3.5f, 4.1f, "Suit O<sub>2</sub> pressure is too low!", "Suit O<sub>2</sub> pressure is too high!") },
            { AlertType.CO2SuitPressure, new Alert(0.0f, 0.1f, "Suit CO<sub>2</sub> pressure is too low!", "Suit CO<sub>2</sub> pressure is too high!") },
            { AlertType.OtherSuitPressure, new Alert(0.0f, 0.5f, "The other gas pressures are too low!", "The other gas pressures are too high!") },
            { AlertType.TotalSuitPressure, new Alert(3.5f, 4.5f, "The total suit pressure is too low!", "The total suit pressure is too high!") },
            { AlertType.CO2HelmetPressure, new Alert(0.0f, 0.15f, "Helmet CO<sub>2</sub> pressure is too low!", "Helmet CO<sub>2</sub> pressure is too high!") },
            { AlertType.PrimaryFanRPM, new Alert(20000, 30001, "Primary fan is spinning too slow!", "Primary fan is spinning too fast!") },         // Upper bound is 30001 (should be 30000) cuz fans hover at 30001 for some reason (phenomenal system NASA)
            { AlertType.SecondaryFanRPM, new Alert(20000, 30001, "Secondary fan is spinning too slow!", "Secondary fan is spinning too fast!") },   // Upper bound is 30001 (should be 30000) cuz fans hover at 30001 for some reason (phenomenal system NASA)
            { AlertType.CO2ScrubberAStorage, new Alert(0, 60, "Scrubber A storage is too low!", "Scrubber A storage is too high!") },
            { AlertType.CO2ScrubberBStorage, new Alert(0, 60, "Scrubber B storage is too low!", "Scrubber B storage is too high!") },
            { AlertType.Temperature, new Alert(50, 90, "Internal temperature is too low!", "Internal temperature is too high!") }
        };
    }

    private void CheckForAlerts(TelemetryData data)
    {
        if (Alerts[AlertType.HeartRate].CheckAlert((float)data.heart_rate))
        {
            MIKENotificationManager.Main.SendNotification("ALERT", Alerts[AlertType.HeartRate].GetAlertMessage((float)data.heart_rate), MIKEResources.Main.NegativeNotificationColor, 5f);
        }

        if (Alerts[AlertType.O2SuitPressure].CheckAlert((float)data.suit_pressure_oxy))
        {
            MIKENotificationManager.Main.SendNotification("ALERT", Alerts[AlertType.O2SuitPressure].GetAlertMessage((float)data.suit_pressure_oxy), MIKEResources.Main.NegativeNotificationColor, 5f);
        }

        if (Alerts[AlertType.CO2SuitPressure].CheckAlert((float)data.suit_pressure_co2))
        {
            MIKENotificationManager.Main.SendNotification("ALERT", Alerts[AlertType.CO2SuitPressure].GetAlertMessage((float)data.suit_pressure_co2), MIKEResources.Main.NegativeNotificationColor, 5f);
        }

        if (Alerts[AlertType.OtherSuitPressure].CheckAlert((float)data.suit_pressure_other))
        {
            MIKENotificationManager.Main.SendNotification("ALERT", Alerts[AlertType.OtherSuitPressure].GetAlertMessage((float)data.suit_pressure_other), MIKEResources.Main.NegativeNotificationColor, 5f);
        }

        if (Alerts[AlertType.TotalSuitPressure].CheckAlert((float)data.suit_pressure_total))
        {
            MIKENotificationManager.Main.SendNotification("ALERT", Alerts[AlertType.TotalSuitPressure].GetAlertMessage((float)data.suit_pressure_total), MIKEResources.Main.NegativeNotificationColor, 5f);
        }

        if (Alerts[AlertType.CO2HelmetPressure].CheckAlert((float)data.helmet_pressure_co2))
        {
            MIKENotificationManager.Main.SendNotification("ALERT", Alerts[AlertType.CO2HelmetPressure].GetAlertMessage((float)data.helmet_pressure_co2), MIKEResources.Main.NegativeNotificationColor, 5f);
        }

        if (Alerts[AlertType.PrimaryFanRPM].CheckAlert((float)data.fan_pri_rpm) && MIKESystemManager.Main.SystemStatuses[SystemType.Fan].GetActiveStatus() == "Primary Fan") // Special case for primary fan
        {
            MIKENotificationManager.Main.SendNotification("ALERT", Alerts[AlertType.PrimaryFanRPM].GetAlertMessage((float)data.fan_pri_rpm), MIKEResources.Main.NegativeNotificationColor, 5f);
        }
        else if (Alerts[AlertType.SecondaryFanRPM].CheckAlert((float)data.fan_sec_rpm) && MIKESystemManager.Main.SystemStatuses[SystemType.Fan].GetActiveStatus() == "Secondary Fan") // Special case for secondary fan
        {
            MIKENotificationManager.Main.SendNotification("ALERT", Alerts[AlertType.SecondaryFanRPM].GetAlertMessage((float)data.fan_sec_rpm), MIKEResources.Main.NegativeNotificationColor, 5f);
        }

        if (Alerts[AlertType.CO2ScrubberAStorage].CheckAlert((float)data.scrubber_a_co2_storage) && MIKESystemManager.Main.SystemStatuses[SystemType.CO2].GetActiveStatus() == "Scrubber A") // Special case for scrubber A
        {
            MIKENotificationManager.Main.SendNotification("ALERT", Alerts[AlertType.CO2ScrubberAStorage].GetAlertMessage((float)data.scrubber_a_co2_storage), MIKEResources.Main.NegativeNotificationColor, 5f);
        }
        else if (Alerts[AlertType.CO2ScrubberBStorage].CheckAlert((float)data.scrubber_b_co2_storage) && MIKESystemManager.Main.SystemStatuses[SystemType.CO2].GetActiveStatus() == "Scrubber B") // Special case for scrubber B
        {
            MIKENotificationManager.Main.SendNotification("ALERT", Alerts[AlertType.CO2ScrubberBStorage].GetAlertMessage((float)data.scrubber_b_co2_storage), MIKEResources.Main.NegativeNotificationColor, 5f);
        }

        if (Alerts[AlertType.Temperature].CheckAlert((float)data.temperature))
        {
            MIKENotificationManager.Main.SendNotification("ALERT", Alerts[AlertType.Temperature].GetAlertMessage((float)data.temperature), MIKEResources.Main.NegativeNotificationColor, 5f);
        }
    }
}

public class Alert
{
    public bool Active { get; private set; }
    public bool OutsideUpperBound { get; private set; } // True if active and above max value, false if active and below min value
    public float MinValue { get; private set; }
    public float MaxValue { get; private set; }
    public string UpperMessage { get; private set; }
    public string LowerMessage { get; private set; }

    private bool displayed = false;

    public Alert(float minValue, float maxValue, string lowerMessage, string upperMessage)
    {
        MinValue = minValue;
        MaxValue = maxValue;
        LowerMessage = lowerMessage;
        UpperMessage = upperMessage;
    }

    public bool CheckAlert(float value)
    {
        if (value < MinValue || value > MaxValue)
        {
            Active = true;
            if (value > MaxValue)
                OutsideUpperBound = true;
            else
                OutsideUpperBound = false;
        }
        else
            Active = false;

        if (Active && !displayed)
        {
            displayed = true;
            return true;
        }
        else if (!Active)
        {
            displayed = false;
        }

        return false;
    }

    public string GetAlertMessage(float value)
    {
        if (value < MinValue)
        {
            return LowerMessage;
        }
        else if (value > MaxValue)
        {
            return UpperMessage;
        }
        else
        {
            return null;
        }
    }
}
