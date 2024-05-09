using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitalsScreen : LMCCScreen
{
    [Header("Resources")]
    [SerializeField] private MIKEVitalsWidgetValue batteryTimeLeft;
    [SerializeField] private MIKEVitalsWidgetValue O2timeLeft;
    [SerializeField] private MIKEVitalsWidgetValue O2PrimaryStorage;
    [SerializeField] private MIKEVitalsWidgetValue O2PrimaryPressure;
    [SerializeField] private MIKEVitalsWidgetValue O2SecondaryStorage;
    [SerializeField] private MIKEVitalsWidgetValue O2SecondaryPressure;
    [SerializeField] private MIKEVitalsWidgetValue coolant;

    [Header("Suit")]
    [SerializeField] private MIKEVitalsWidgetValue heartRate;
    [SerializeField] private MIKEVitalsWidgetValue O2Consumption;
    [SerializeField] private MIKEVitalsWidgetValue CO2Production;
    [SerializeField] private MIKEVitalsWidgetValue suitO2Pressure;
    [SerializeField] private MIKEVitalsWidgetValue suitCO2Pressure;
    [SerializeField] private MIKEVitalsWidgetValue suitOtherPressure;
    [SerializeField] private MIKEVitalsWidgetValue suitTotalPressure;
    [SerializeField] private MIKEVitalsWidgetValue helmetCO2Pressure;

    [Header("Fans")]
    [SerializeField] private MIKEVitalsWidgetValue fanPrimary;
    [SerializeField] private MIKEVitalsWidgetValue fanSecondary;

    [Header("Misc")]
    [SerializeField] private MIKEVitalsWidgetValue scrubberAPressure;
    [SerializeField] private MIKEVitalsWidgetValue scrubberBPressure;
    [SerializeField] private MIKEVitalsWidgetValue temperature;
    [SerializeField] private MIKEVitalsWidgetValue H20GasPressure;
    [SerializeField] private MIKEVitalsWidgetValue H20LiquidPressure;


    private Dictionary<MIKEVitalsWidgetValue, ErrorScenario> errorScenarios;

    // Start is called before the first frame update
    void Start()
    {
        errorScenarios = new Dictionary<MIKEVitalsWidgetValue, ErrorScenario>();
        SetWidgetValues();
        CreateErrorScenarios();
        TSSManager.Main.OnTelemetryUpdated += HandleVitalsData;
        TSSManager.Main.OnDCUUpdated += UpdateActiveDevices;
    }

    private void HandleVitalsData(TelemetryData data)
    {
        UpdateVitals(data);
        CheckForCriticalVitals();
    }

    private void UpdateActiveDevices(DCUData data)
    {
        errorScenarios[fanPrimary].deviceActive = data.fan;
        errorScenarios[fanSecondary].deviceActive = !data.fan;

        errorScenarios[scrubberAPressure].deviceActive = data.co2;
        errorScenarios[scrubberBPressure].deviceActive = !data.co2;
    }

    private void UpdateVitals(TelemetryData data)
    {
        batteryTimeLeft.SetValue((float)TSSManager.Main.TelemetryData.batt_time_left);
        O2timeLeft.SetValue(TSSManager.Main.TelemetryData.oxy_time_left);
        O2PrimaryStorage.SetValue((float)TSSManager.Main.TelemetryData.oxy_pri_storage);
        O2PrimaryPressure.SetValue((float)TSSManager.Main.TelemetryData.oxy_pri_pressure);
        O2SecondaryStorage.SetValue((float)TSSManager.Main.TelemetryData.oxy_sec_storage);
        O2SecondaryPressure.SetValue((float)TSSManager.Main.TelemetryData.oxy_sec_pressure);
        coolant.SetValue((float)TSSManager.Main.TelemetryData.coolant_ml);

        heartRate.SetValue((float)TSSManager.Main.TelemetryData.heart_rate);
        O2Consumption.SetValue((float)TSSManager.Main.TelemetryData.oxy_consumption);
        CO2Production.SetValue((float)TSSManager.Main.TelemetryData.co2_production);
        suitO2Pressure.SetValue((float)TSSManager.Main.TelemetryData.suit_pressure_oxy);
        suitCO2Pressure.SetValue((float)TSSManager.Main.TelemetryData.suit_pressure_co2);
        suitOtherPressure.SetValue((float)TSSManager.Main.TelemetryData.suit_pressure_other);
        suitTotalPressure.SetValue((float)TSSManager.Main.TelemetryData.suit_pressure_total);
        helmetCO2Pressure.SetValue((float)TSSManager.Main.TelemetryData.helmet_pressure_co2);

        fanPrimary.SetValue((float)TSSManager.Main.TelemetryData.fan_pri_rpm);
        fanSecondary.SetValue((float)TSSManager.Main.TelemetryData.fan_sec_rpm);

        scrubberAPressure.SetValue((float)TSSManager.Main.TelemetryData.scrubber_a_co2_storage);
        scrubberBPressure.SetValue((float)TSSManager.Main.TelemetryData.scrubber_b_co2_storage);
        temperature.SetValue((float)TSSManager.Main.TelemetryData.temperature);
        H20GasPressure.SetValue((float)TSSManager.Main.TelemetryData.coolant_gas_pressure);
        H20LiquidPressure.SetValue((float)TSSManager.Main.TelemetryData.coolant_liquid_pressure);
    }

    private void CheckForCriticalVitals()
    {
        foreach (KeyValuePair<MIKEVitalsWidgetValue, ErrorScenario> entry in errorScenarios)
        {
            MIKEVitalsWidgetValue widget = entry.Key;
            ErrorScenario scenario = entry.Value;

            if (scenario.deviceActive.HasValue && !scenario.deviceActive.Value)
            {
                continue;
            }

            if (widget.CurrentValue > widget.MaxValue)
            {
                // Create a critical message
                MIKENotificationManager.Main.SendNotification("NOTIFICATION", scenario.upperLimitMessage, MIKEResources.Main.NegativeNotificationColor, 5f);
            }
            else if (widget.CurrentValue < widget.MinValue)
            {
                // Create a critical message
                MIKENotificationManager.Main.SendNotification("NOTIFICATION", scenario.lowerLimitMessage, MIKEResources.Main.NegativeNotificationColor, 5f);
            }
            else
            {
                // Hide the critical message
            }
        }
    }

    private void SetWidgetValues()
    {
        batteryTimeLeft.SetBounds(3600f, 10800f);
        O2PrimaryStorage.SetBounds(20f, 100f);
        O2SecondaryStorage.SetBounds(20f, 100f);
        O2PrimaryPressure.SetBounds(600f, 3000f);
        O2SecondaryPressure.SetBounds(600f, 3000f);
        O2timeLeft.SetBounds(3600f, 21600f);
        coolant.SetBounds(80f, 100f, 100f);

        heartRate.SetBounds(50f, 160f, 90f);
        O2Consumption.SetBounds(0.05f, 0.15f, 0.1f);
        CO2Production.SetBounds(0.05f, 0.15f, 0.1f);
        suitO2Pressure.SetBounds(3.5f, 4.1f, 4.0f);
        suitCO2Pressure.SetBounds(0.0f, 0.1f, 0.0f);
        suitOtherPressure.SetBounds(0.0f, 0.5f, 0.0f);
        suitTotalPressure.SetBounds(3.5f, 4.5f, 4.0f);
        helmetCO2Pressure.SetBounds(0.0f, 0.15f, 0.1f);

        fanPrimary.SetBounds(20000f, 30000f, 30000f);
        fanSecondary.SetBounds(20000f, 30000f, 30000f);

        scrubberAPressure.SetBounds(0f, 60f);
        scrubberBPressure.SetBounds(0f, 60f);
        temperature.SetBounds(50f, 90f, 70f);
        H20LiquidPressure.SetBounds(100f, 700f, 500f);
        H20GasPressure.SetBounds(0f, 700f, 0f);
    }

    private void CreateErrorScenarios()
    {
        // Heart rate
        errorScenarios.Add(heartRate, new ErrorScenario()
        {
            upperLimitMessage = "Heart rate is too high, please take a rest!",
            lowerLimitMessage = "Heart rate is too low, please exert more energy!", // Should never happen
            upperLimitMessageBrief = "Heart rate too high",
            lowerLimitMessageBrief = "Heart rate too low" // Should never happen
        });

        // Suit O2 pressure
        errorScenarios.Add(suitO2Pressure, new ErrorScenario()
        {
            upperLimitMessage = "Suit O2 pressure is too high, swap to the secondary tank",
            lowerLimitMessage = "Suit O2 pressure is too low, swap to the secondary tank",
            upperLimitMessageBrief = "Suit O2 pressure too high",
            lowerLimitMessageBrief = "Suit O2 pressure too low"
        });

        // Suit CO2 pressure
        errorScenarios.Add(suitCO2Pressure, new ErrorScenario()
        {
            upperLimitMessage = "Suit CO2 pressure is too high, please vent the scrubber",
            lowerLimitMessage = "Suit CO2 pressure is too low, please check the scrubber", // Should never happen
            upperLimitMessageBrief = "Suit CO2 pressure too high",
            lowerLimitMessageBrief = "Suit CO2 pressure too low" // Should never happen
        });

        // Suit total pressure
        errorScenarios.Add(suitTotalPressure, new ErrorScenario()
        {
            upperLimitMessage = "Suit total pressure is too high, check the oxygen tank and scrubber",
            lowerLimitMessage = "Suit total pressure is too low, check the oxygen tank and scrubber",
            upperLimitMessageBrief = "Suit total pressure too high",
            lowerLimitMessageBrief = "Suit total pressure too low"
        });

        // Helmet
        errorScenarios.Add(helmetCO2Pressure, new ErrorScenario()
        {
            upperLimitMessage = "Helmet CO2 pressure is too high, swap to the other fan",
            lowerLimitMessage = "Helmet CO2 pressure is too low, swap to the other fan",
            upperLimitMessageBrief = "Helmet CO2 pressure too high",
            lowerLimitMessageBrief = "Helmet CO2 pressure too low"
        });

        // Fans
        errorScenarios.Add(fanPrimary, new ErrorScenario()
        {
            upperLimitMessage = "Primary fan is spinning too fast, swap to the secondary fan", // Should never happen
            lowerLimitMessage = "Primary fan is not spinning fast enough, swap to the secondary fan",
            upperLimitMessageBrief = "Primary fan spinning too fast", // Should never happen
            lowerLimitMessageBrief = "Primary fan spinning too slow",
            deviceActive = false
        });

        errorScenarios.Add(fanSecondary, new ErrorScenario()
        {
            upperLimitMessage = "Secondary fan is spinning too fast, swap to the primary fan",
            lowerLimitMessage = "Secondary fan is not spinning fast enough, swap to the primary fan",
            upperLimitMessageBrief = "Secondary fan spinning too fast",
            lowerLimitMessageBrief = "Secondary fan spinning too slow",
            deviceActive = false
        });

        // Scrubbers
        errorScenarios.Add(scrubberAPressure, new ErrorScenario()
        {
            upperLimitMessage = "Scrubber A CO2 storage is too high, please vent",
            lowerLimitMessage = "Scrubber A CO2 storage is too low, please vent", // Should never happen
            upperLimitMessageBrief = "Scrubber A pressure too high",
            lowerLimitMessageBrief = "Scrubber A pressure too low", // Should never happen
            deviceActive = false
        });

        errorScenarios.Add(scrubberBPressure, new ErrorScenario()
        {
            upperLimitMessage = "Scrubber B CO2 storage is too high, please vent",
            lowerLimitMessage = "Scrubber B CO2 storage is too low, please vent", // Should never happen
            upperLimitMessageBrief = "Scrubber B pressure too high",
            lowerLimitMessageBrief = "Scrubber B pressure too low", // Should never happen
            deviceActive = false
        });

        // Temperature
        errorScenarios.Add(temperature, new ErrorScenario()
        {
            upperLimitMessage = "Internal temperature is too high, please slow down!",
            lowerLimitMessage = "Internal temperature is too low, please speed up!", // Should never happen
            upperLimitMessageBrief = "Temperature too high",
            lowerLimitMessageBrief = "Temperature too low" // Should never happen
        });
    }
}

public class ErrorScenario
{
    public string upperLimitMessage;
    public string lowerLimitMessage;
    public string upperLimitMessageBrief;
    public string lowerLimitMessageBrief;
    public bool? deviceActive;
}
