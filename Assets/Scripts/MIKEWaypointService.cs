using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MIKEWaypointService : MIKEService
{
    // Start is called before the first frame update
    void Start()
    {
        Service = ServiceType.Waypoint;
        IsReliable = true;
        MIKEInputManager.Main.RegisterService(Service, this);
    }

    public override void ReceiveData(byte[] data)
    {
        List<byte> dataAsList = data.ToList();

        // remove device ID byte and reliability byte
        dataAsList.RemoveRange(0, 2);

        // Parse waypoint ID
        int waypointID = BitConverter.ToInt32(dataAsList.GetRange(0, 4).ToArray(), 0);
        dataAsList.RemoveRange(0, 4);

        // Parse waypoint action
        char waypointAction = BitConverter.ToChar(dataAsList.GetRange(0, sizeof(char)).ToArray(), 0);
        dataAsList.RemoveRange(0, sizeof(char));

        // Parse data
        float xPos = BitConverter.ToSingle(dataAsList.GetRange(0, 4).ToArray(), 0);
        float yPos = BitConverter.ToSingle(dataAsList.GetRange(4, 4).ToArray(), 0);

        Vector3 waypointPos = MIKEMap.Main.GetPositionFromNormalized(new Vector2(xPos, yPos));
        LMCCWaypointSpawner.Main.SpawnNewHUDWaypoint(waypointID, waypointPos);
    }
}
