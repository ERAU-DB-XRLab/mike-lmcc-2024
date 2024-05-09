using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MIKEService : MonoBehaviour
{

    public int serviceID;

    public abstract void ReceiveData(byte[] data);
}
