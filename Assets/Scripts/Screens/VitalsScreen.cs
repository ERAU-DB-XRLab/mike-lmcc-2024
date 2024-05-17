using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VitalsScreen : LMCCScreen
{
    [Header("Icons")]
    [SerializeField] private MIKEIconWidgetValue heartRateIcon;
    [SerializeField] private MIKEIconWidgetValue o2StorageIcon;
    [SerializeField] private MIKEIconWidgetValue suitPSIIcon;
    [SerializeField] private MIKEIconWidgetValue o2PSIIcon;
    [SerializeField] private MIKEIconWidgetValue co2PSIIcon;
    [SerializeField] private MIKEIconWidgetValue otherPSIIcon;
    [SerializeField] private MIKEIconWidgetValue fanRPMIcon;
    [SerializeField] private MIKEIconWidgetValue temperatureIcon;
    [SerializeField] private MIKEIconWidgetValue helmetCO2Icon;
    [SerializeField] private MIKEIconWidgetValue scrubberCO2StorageIcon;

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
    [SerializeField] private MIKEVitalsWidgetValue scrubberAStorage;
    [SerializeField] private MIKEVitalsWidgetValue scrubberBStorage;
    [SerializeField] private MIKEVitalsWidgetValue temperature;
    [SerializeField] private MIKEVitalsWidgetValue coolantGasPressure;
    [SerializeField] private MIKEVitalsWidgetValue coolantLiquidPressure;
    [Space]
    [SerializeField] private GameObject errorBlockPrefab;
    [SerializeField] private Transform errorBlockParent;
    [SerializeField] private ContentSizeFitter fitter;


    private Dictionary<AlertType, ErrorScenario> errorScenarios;
    private Dictionary<AlertType, LMCCErrorBlock> errorBlocks;


    // Start is called before the first frame update
    void Start()
    {
        errorScenarios = new Dictionary<AlertType, ErrorScenario>();
        errorBlocks = new Dictionary<AlertType, LMCCErrorBlock>();
        SetWidgetBounds();
        CreateErrorScenarios();
        TSSManager.Main.OnTelemetryUpdated += HandleVitalsData;
        TSSManager.Main.OnDCUUpdated += UpdateActiveDevices;
    }

    private void HandleVitalsData(TelemetryData data)
    {
        UpdateVitals(data);
        UpdateIconVitals(data);
        CheckForCriticalVitals();
    }

    private void UpdateVitals(TelemetryData data)
    {
        batteryTimeLeft.SetValue((float)data.batt_time_left);
        O2timeLeft.SetValue(data.oxy_time_left);
        O2PrimaryStorage.SetValue((float)data.oxy_pri_storage);
        O2PrimaryPressure.SetValue((float)data.oxy_pri_pressure);
        O2SecondaryStorage.SetValue((float)data.oxy_sec_storage);
        O2SecondaryPressure.SetValue((float)data.oxy_sec_pressure);
        coolant.SetValue((float)data.coolant_ml);

        heartRate.SetValue((float)data.heart_rate);
        O2Consumption.SetValue((float)data.oxy_consumption);
        CO2Production.SetValue((float)data.co2_production);
        suitO2Pressure.SetValue((float)data.suit_pressure_oxy);
        suitCO2Pressure.SetValue((float)data.suit_pressure_co2);
        suitOtherPressure.SetValue((float)data.suit_pressure_other);
        suitTotalPressure.SetValue((float)data.suit_pressure_total);
        helmetCO2Pressure.SetValue((float)data.helmet_pressure_co2);

        fanPrimary.SetValue((float)data.fan_pri_rpm);
        fanSecondary.SetValue((float)data.fan_sec_rpm);

        scrubberAStorage.SetValue((float)data.scrubber_a_co2_storage);
        scrubberBStorage.SetValue((float)data.scrubber_b_co2_storage);
        temperature.SetValue((float)data.temperature);
        coolantGasPressure.SetValue((float)data.coolant_gas_pressure);
        coolantLiquidPressure.SetValue((float)data.coolant_liquid_pressure);
    }

    private void UpdateIconVitals(TelemetryData data)
    {
        heartRateIcon.SetValue((float)data.heart_rate);
        o2StorageIcon.SetValue(MIKESystemManager.Main.SystemStatuses[SystemType.Oxygen].GetActiveStatus() == "Primary Tank" ? (float)data.oxy_pri_storage : (float)data.oxy_sec_storage);
        suitPSIIcon.SetValue((float)data.suit_pressure_total);
        o2PSIIcon.SetValue((float)data.suit_pressure_oxy);
        co2PSIIcon.SetValue((float)data.suit_pressure_co2);
        otherPSIIcon.SetValue((float)data.suit_pressure_other);
        fanRPMIcon.SetValue(MIKESystemManager.Main.SystemStatuses[SystemType.Fan].GetActiveStatus() == "Primary Fan" ? (float)data.fan_pri_rpm / 1000f : (float)data.fan_sec_rpm / 1000f);
        temperatureIcon.SetValue((float)data.temperature);
        helmetCO2Icon.SetValue((float)data.helmet_pressure_co2);
        scrubberCO2StorageIcon.SetValue(MIKESystemManager.Main.SystemStatuses[SystemType.CO2].GetActiveStatus() == "Scrubber A" ? (float)data.scrubber_a_co2_storage : (float)data.scrubber_b_co2_storage);
    }

    private void UpdateActiveDevices(DCUData data)
    {
        errorScenarios[AlertType.PrimaryFanRPM].deviceActive = data.fan;
        errorScenarios[AlertType.SecondaryFanRPM].deviceActive = !data.fan;

        errorScenarios[AlertType.CO2ScrubberAStorage].deviceActive = data.co2;
        errorScenarios[AlertType.CO2ScrubberBStorage].deviceActive = !data.co2;
    }

    private void CheckForCriticalVitals()
    {
        foreach (KeyValuePair<AlertType, Alert> entry in MIKEAlertManager.Main.Alerts)
        {
            DisplayAlert(entry.Key, entry.Value.Active && errorScenarios[entry.Key].deviceActive, entry.Value.OutsideUpperBound);
        }
    }

    private void DisplayAlert(AlertType type, bool value, bool upperBound)
    {
        if (value)
        {
            if (!errorBlocks.ContainsKey(type))
            {
                LMCCErrorBlock block = Instantiate(errorBlockPrefab, errorBlockParent).GetComponent<LMCCErrorBlock>();
                block.SetError(errorScenarios[type].title, upperBound ? errorScenarios[type].upperLimitMessage : errorScenarios[type].lowerLimitMessage);
                errorBlocks.Add(type, block);
            }
        }
        else
        {
            if (errorBlocks.ContainsKey(type))
            {
                Destroy(errorBlocks[type].gameObject);
                errorBlocks.Remove(type);
            }
        }
    }

    private void SetWidgetBounds()
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

        fanPrimary.SetBounds(20000f, 30000f, 30001f);
        fanSecondary.SetBounds(20000f, 30000f, 30001f);

        scrubberAStorage.SetBounds(0f, 60f);
        scrubberBStorage.SetBounds(0f, 60f);
        temperature.SetBounds(50f, 90f, 70f);
        coolantLiquidPressure.SetBounds(100f, 700f, 500f);
        coolantGasPressure.SetBounds(0f, 700f, 0f);
    }

    private void CreateErrorScenarios()
    {
        // Heart rate
        errorScenarios.Add(AlertType.HeartRate, new ErrorScenario()
        {
            title = "Heart Rate",
            upperLimitMessage = "Heart rate is too high, please take a rest!",
            lowerLimitMessage = "Heart rate is too low, please exert more energy!", // Should never happen
            deviceActive = true
        });

        // Suit O2 pressure
        errorScenarios.Add(AlertType.O2SuitPressure, new ErrorScenario()
        {
            title = "Suit O2 Pressure",
            upperLimitMessage = "Suit O2 pressure is too high, swap to the secondary tank",
            lowerLimitMessage = "Suit O2 pressure is too low, swap to the secondary tank",
            deviceActive = true
        });

        // Suit CO2 pressure
        errorScenarios.Add(AlertType.CO2SuitPressure, new ErrorScenario()
        {
            title = "Suit CO2 Pressure",
            upperLimitMessage = "Suit CO2 pressure is too high, please vent the scrubber",
            lowerLimitMessage = "Suit CO2 pressure is too low, please check the scrubber", // Should never happen
            deviceActive = true
        });

        // Suit other pressure
        errorScenarios.Add(AlertType.OtherSuitPressure, new ErrorScenario()
        {
            title = "Other Suit Pressure",
            upperLimitMessage = "The other gas pressures in the suit are too high after decompression, please alert the HUD",
            lowerLimitMessage = "The other gas pressures in the suit are too low after decompression, please alert the HUD", // Should never happen
            deviceActive = true
        });

        // Suit total pressure
        errorScenarios.Add(AlertType.TotalSuitPressure, new ErrorScenario()
        {
            title = "Total Suit Pressure",
            upperLimitMessage = "Suit total pressure is too high, check the oxygen tank and scrubber",
            lowerLimitMessage = "Suit total pressure is too low, check the oxygen tank and scrubber",
            deviceActive = true
        });

        // Helmet
        errorScenarios.Add(AlertType.CO2HelmetPressure, new ErrorScenario()
        {
            title = "Helmet CO2 Pressure",
            upperLimitMessage = "Helmet CO2 pressure is too high, swap to the other fan",
            lowerLimitMessage = "Helmet CO2 pressure is too low, swap to the other fan",
            deviceActive = true
        });

        // Fans
        errorScenarios.Add(AlertType.PrimaryFanRPM, new ErrorScenario()
        {
            title = "Primary Fan RPM",
            upperLimitMessage = "Primary fan is spinning too fast, swap to the secondary fan", // Should never happen
            lowerLimitMessage = "Primary fan is not spinning fast enough, swap to the secondary fan",
            deviceActive = false
        });

        errorScenarios.Add(AlertType.SecondaryFanRPM, new ErrorScenario()
        {
            title = "Secondary Fan RPM",
            upperLimitMessage = "Secondary fan is spinning too fast, swap to the primary fan", // Should never happen
            lowerLimitMessage = "Secondary fan is not spinning fast enough, swap to the primary fan",
            deviceActive = false
        });

        // Scrubbers
        errorScenarios.Add(AlertType.CO2ScrubberAStorage, new ErrorScenario()
        {
            title = "Scrubber A Storage",
            upperLimitMessage = "Scrubber A CO2 storage is too high, please vent",
            lowerLimitMessage = "Scrubber A CO2 storage is too low, please vent", // Should never happen
            deviceActive = false
        });

        errorScenarios.Add(AlertType.CO2ScrubberBStorage, new ErrorScenario()
        {
            title = "Scrubber B Storage",
            upperLimitMessage = "Scrubber B CO2 storage is too high, please vent",
            lowerLimitMessage = "Scrubber B CO2 storage is too low, please vent", // Should never happen
            deviceActive = false
        });

        // Temperature
        errorScenarios.Add(AlertType.Temperature, new ErrorScenario()
        {
            title = "Temperature",
            upperLimitMessage = "Internal temperature is too high, please slow down!",
            lowerLimitMessage = "Internal temperature is too low, please speed up!", // Should never happen
            deviceActive = true
        });
    }
}

public class ErrorScenario
{
    public string title;
    public string upperLimitMessage;
    public string lowerLimitMessage;
    public bool deviceActive;
}
