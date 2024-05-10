using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MIKEPacket packet = new MIKEPacket();
        packet.Write("Hello!擅");
        packet.Write('擅');
        packet.ReadString();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
