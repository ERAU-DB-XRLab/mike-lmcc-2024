using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public enum EVA
{
    EVA1,
    EVA2
}

public class TSSManager : MonoBehaviour
{
    public delegate void Connection();
    public event Connection OnConnected;
    public delegate void Disconnection();
    public event Disconnection OnDisconnected;

    public delegate void UIAUpdated(UIAData data);
    public event UIAUpdated OnUIAUpdated;

    public delegate void DCUUpdated(DCUData data);
    public event DCUUpdated OnDCUUpdated;

    public delegate void RoverUpdated(RoverData data);
    public event RoverUpdated OnRoverUpdated;

    public delegate void SpecUpdated(SpecData data);
    public event SpecUpdated OnSpecUpdated;

    public delegate void TelemetryUpdated(TelemetryData data);
    public event TelemetryUpdated OnTelemetryUpdated;

    public delegate void CommUpdated(CommData data);
    public event CommUpdated OnCommUpdated;

    public delegate void IMUUpdated(IMUData data);
    public event IMUUpdated OnIMUUpdated;


    public static TSSManager Main { get; private set; }
    public bool Connected { get { return TSSc.connected; } }

    public string Host { get { return host; } set { host = value; } }
    public EVA CurrentEVA { get { return currentEVA; } set { currentEVA = value; } }

    public double EVATime { get; private set; }
    public UIAData UIAData { get; private set; }
    public DCUData DCUData { get; private set; }
    public RoverData RoverData { get; private set; }
    public SpecData SpecData { get; private set; }
    public TelemetryData TelemetryData { get; private set; }
    public CommData CommData { get; private set; }
    public IMUData IMUData { get; private set; }

    [SerializeField] private string host = "127.0.0.1";
    [SerializeField] private EVA currentEVA;

    private bool currentCalledEvent = false; // True is OnConnected, False is OnDisconnected

    private JsonSerializerSettings settings = new JsonSerializerSettings
    {
        MissingMemberHandling = MissingMemberHandling.Error
    };

    private TSScConnection TSSc;

    void Awake()
    {
        if (Main == null)
            Main = this;
        else
            Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        TSSc = GetComponent<TSScConnection>();
        TSSc.ConnectToHost(host, MIKEResources.Main.TeamNumber);
    }

    void OnDisable()
    {
        TSSc.DisconnectFromHost();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the UIA data has been updated
        if (TSSc.isUIAUpdated())
        {
            //Debug.Log("UIA Updated");
            UIAData = JsonConvert.DeserializeObject<UIAWrapper>(TSSc.GetUIAJsonString(), settings).uia;
            OnUIAUpdated?.Invoke(UIAData);
        }

        // Check if the DCU data has been updated
        if (TSSc.isDCUUpdated())
        {
            DCU_EVAData temp = JsonConvert.DeserializeObject<DCUWrapper>(TSSc.GetDCUJsonString()).dcu;
            DCUData = CurrentEVA == EVA.EVA1 ? temp.eva1 : temp.eva2;
            OnDCUUpdated?.Invoke(DCUData);
        }

        // Check if the ROVER data has been updated
        if (TSSc.isROVERUpdated())
        {
            //Debug.Log("ROVER Updated");
            RoverData = JsonConvert.DeserializeObject<RoverWrapper>(TSSc.GetROVERJsonString()).rover;
            OnRoverUpdated?.Invoke(RoverData);
        }

        // Check if the SPEC data has been updated
        if (TSSc.isSPECUpdated())
        {
            //Debug.Log("SPEC Updated");
            Spec_EVAData temp = JsonConvert.DeserializeObject<SpecWrapper>(TSSc.GetSPECJsonString()).spec;
            SpecData = CurrentEVA == EVA.EVA1 ? temp.eva1 : temp.eva2;
            OnSpecUpdated?.Invoke(SpecData);
        }

        // Check if the TELEMETRY data has been updated
        if (TSSc.isTELEMETRYUpdated())
        {
            //Debug.Log("TELEMETRY Updated");
            Telemetry_EVAData temp = JsonConvert.DeserializeObject<TelemetryWrapper>(TSSc.GetTELEMETRYJsonString()).telemetry;
            EVATime = temp.eva_time;
            TelemetryData = CurrentEVA == EVA.EVA1 ? temp.eva1 : temp.eva2;
            OnTelemetryUpdated?.Invoke(TelemetryData);
        }

        // Check if the COMM data has been updated
        if (TSSc.isCOMMUpdated())
        {
            //Debug.Log("COMM Updated");
            CommData = JsonConvert.DeserializeObject<CommWrapper>(TSSc.GetCOMMJsonString()).comm;
            OnCommUpdated?.Invoke(CommData);
        }

        // Check if the IMU data has been updated
        if (TSSc.isIMUUpdated())
        {
            //Debug.Log("IMU Updated");
            IMUData = JsonConvert.DeserializeObject<IMUWrapper>(TSSc.GetIMUJsonString()).imu;
            OnIMUUpdated?.Invoke(IMUData);
        }

        // Check the connection status
        if (Connected && !currentCalledEvent)
        {
            currentCalledEvent = true;
            OnConnected?.Invoke();
        }
        else if (!Connected && currentCalledEvent)
        {
            currentCalledEvent = false;
            OnDisconnected?.Invoke();
        }
    }
}

public class UIAWrapper
{
    public UIAData uia;
}

[Serializable]
public class UIAData
{
    public bool eva1_power;
    public bool eva1_oxy;
    public bool eva1_water_supply;
    public bool eva1_water_waste;
    public bool eva2_power;
    public bool eva2_oxy;
    public bool eva2_water_supply;
    public bool eva2_water_waste;
    public bool oxy_vent;
    public bool depress;
}

public class DCUWrapper
{
    public DCU_EVAData dcu;
}

[Serializable]
public class DCU_EVAData
{
    public DCUData eva1;
    public DCUData eva2;
}

[Serializable]
public class DCUData
{
    public bool batt;
    public bool oxy;
    public bool comm;
    public bool fan;
    public bool pump;
    public bool co2;
}

public class RoverWrapper
{
    public RoverData rover;
}

[Serializable]
public class RoverData
{
    public double posx;
    public double posy;
    public int qr_id;
}

public class SpecWrapper
{
    public Spec_EVAData spec;
}

public class Spec_EVAData
{
    public SpecData eva1;
    public SpecData eva2;
}

[Serializable]
public class SpecData
{
    public string name;
    public int id;
    public Data data;

    public class Data
    {
        public int SiO2;
        public int TiO2;
        public int Al2O3;
        public int FeO;
        public int MnO;
        public int MgO;
        public int CaO;
        public int K2O;
        public int P2O3;
        public int other;
    }
}

public class TelemetryWrapper
{
    public Telemetry_EVAData telemetry;
}

[Serializable]
public class Telemetry_EVAData
{
    public double eva_time;
    public TelemetryData eva1;
    public TelemetryData eva2;
}

[Serializable]
public class TelemetryData
{
    public double batt_time_left;
    public double oxy_pri_storage;
    public double oxy_sec_storage;
    public double oxy_pri_pressure;
    public double oxy_sec_pressure;
    public int oxy_time_left;
    public double heart_rate;
    public double oxy_consumption;
    public double co2_production;
    public double suit_pressure_oxy;
    public double suit_pressure_co2;
    public double suit_pressure_other;
    public double suit_pressure_total;
    public double fan_pri_rpm;
    public double fan_sec_rpm;
    public double helmet_pressure_co2;
    public double scrubber_a_co2_storage;
    public double scrubber_b_co2_storage;
    public double temperature;
    public double coolant_ml;
    public double coolant_gas_pressure;
    public double coolant_liquid_pressure;
}

public class CommWrapper
{
    public CommData comm;
}

[Serializable]
public class CommData
{
    public bool comm_tower;
}

public class IMUWrapper
{
    [JsonRequired]
    public IMUData imu;
}

[Serializable]
public class IMUData
{
    [JsonRequired]
    public Data eva1;
    [JsonRequired]
    public Data eva2;

    public class Data
    {
        [JsonRequired]
        public double posx;
        [JsonRequired]
        public double posy;
        [JsonRequired]
        public double heading;
    }

    public Data YourEVA
    {
        get
        {
            return TSSManager.Main.CurrentEVA == EVA.EVA1 ? eva1 : eva2;
        }
    }

    public Data OtherEVA
    {
        get
        {
            return TSSManager.Main.CurrentEVA == EVA.EVA1 ? eva2 : eva1;
        }
    }
}
