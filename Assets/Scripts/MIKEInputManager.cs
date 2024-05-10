using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MIKEInputManager : MonoBehaviour
{

    public static MIKEInputManager Main { get; private set; }

    // SERVICES
    private Dictionary<int, MIKEService> services = new Dictionary<int, MIKEService>();

    private int otherReliableCounter = 0;

    void Awake()
    {
        if (Main == null)
            Main = this;
        else
            Destroy(this);
    }

    public void RegisterService(ServiceType type, MIKEService service)
    {
        services.Add((int)type, service);
    }

    public void ReceiveInput(byte[] data)
    {
        if (data == null || data.Length == 0)
        {
            Debug.Log("Empty data received");
            return;
        }

        int id = data[0];

        // First check if it's a service
        if (services.ContainsKey(id))
        {
            var packet = new MIKEPacket(data);
            packet.CurrentIndex++;
            if (services[id].IsReliable)
            {
                int rc = packet.ReadInt();
                if (rc > otherReliableCounter)
                {
                    otherReliableCounter = rc;
                    services[id].ReceiveData(packet);
                }
            }
            else
            {
                services[id].ReceiveData(packet);
            }
            return;
        }
        else
        {
            Debug.Log("Service " + id + " not found");
        }
    }
}
