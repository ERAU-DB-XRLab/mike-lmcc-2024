using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MIKEInputManager : MonoBehaviour
{

    public static MIKEInputManager Main { get; private set; }

    // INPUTS
    //public UnityEvent Select;

    // DEVICE REGISTRATION

    //[SerializeField] private Transform devicesParent;

    private readonly Dictionary<int, string> deviceType = new Dictionary<int, string>();

    // Input devices
    //private Dictionary<int, MIKEInputDeviceEntry> inputDeviceEntries = new Dictionary<int, MIKEInputDeviceEntry>();
    private Dictionary<int, MIKEService> services = new Dictionary<int, MIKEService>();

    private int otherReliableCounter = 0;

    void Awake()
    {
        Main = this;
        deviceType.Add(0, "Console");
        deviceType.Add(2, "Camera");
        deviceType.Add(4, "LMCC");
        deviceType.Add(5, "HUD");
    }

    /*public void RegisterInputDevice(int id)
    {

        string type = deviceType[id];

        // Create new entry
        MIKEInputDeviceEntry entry = Instantiate(Resources.Load<GameObject>(type + "Entry"), devicesParent).GetComponent<MIKEInputDeviceEntry>();
        entry.Init(id, type);
        entry.Disconnected.AddListener(DeviceDisconnected);

        // Populate dictionary
        inputDeviceEntries.Add(id, entry);

        // Notify user
        MIKENotificationManager.Main.SendNotification("NOTIFICATION", "New Input Device Connected!", MIKEResources.Main.PositiveNotificationColor, 2.5f);

    }*/

    public void RegisterService(ServiceType type, MIKEService service)
    {
        services.Add((int)type, service);
    }

    public void ReceiveInput(byte[] data)
    {

        int id = data[0];
        Debug.Log("ID: " + id + " | Count: " + data.Length);

        // First check if it's a service
        if (services.ContainsKey(id))
        {
            if (services[id].IsReliable)
            {
                int rc = data[1];
                if (rc > otherReliableCounter)
                {
                    otherReliableCounter = rc;
                    services[id].ReceiveData(data);
                }
            }
            else
            {
                services[id].ReceiveData(data);
            }
            return;
        }
        else
        {
            Debug.Log("Service " + id + " not found");
        }

        // If not a service, then handle it as an input device

        /*if (!inputDeviceEntries.ContainsKey(deviceID) && deviceType.ContainsKey(deviceID))
        {
            RegisterInputDevice(deviceID);
        }

        if (inputDeviceEntries.ContainsKey(deviceID))
            inputDeviceEntries[deviceID].ReceiveData(data);*/

    }

    /*public void DeviceDisconnected(int id)
    {
        MIKENotificationManager.Main.SendNotification("NOTIFICATION", "Input Device Disconnected", MIKEResources.Main.NegativeNotificationColor, 2.5f);
        Destroy(inputDeviceEntries[id].gameObject);
        inputDeviceEntries.Remove(id);
    }*/

    /*public void CloseAllEntries(MIKEInputDeviceEntry exception)
    {
        foreach (MIKEInputDeviceEntry e in inputDeviceEntries.Values)
        {
            if (e != exception && e.IsExpanded())
                e.SetExpanded(false);
        }

        exception.transform.SetAsFirstSibling();

    }*/

}
