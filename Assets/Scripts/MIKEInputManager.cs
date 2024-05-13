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
    private int reliablePacketCount;

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

        // Check if it's a service
        if (services.ContainsKey(id))
        {
            var packet = new MIKEPacket(data);
            packet.CurrentIndex++;
            if (services[id].IsReliable)
            {
                int rc = packet.ReadInt();
                if (rc > otherReliableCounter)
                {
                    reliablePacketCount = 1;
                    otherReliableCounter = rc;
                    services[id].ReceiveData(packet);
                }
                else if (rc == otherReliableCounter)
                {
                    reliablePacketCount++;
                }
                else
                {
                    Debug.LogWarning("Received an old reliable packet with rc: " + rc);
                }

                if (reliablePacketCount == MIKEServerManager.Main.ReliableSendCount)
                {
                    Debug.Log("No reliable packets lost!");
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
