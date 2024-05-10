using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ServiceType
{
    Audio = 1,
    Waypoint = 6,
}

public abstract class MIKEService : MonoBehaviour
{
    public ServiceType Service { get; protected set; }
    public bool IsReliable { get; protected set; }
    public abstract void ReceiveData(byte[] data);

}
