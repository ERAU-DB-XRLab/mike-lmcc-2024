using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum WaypointServiceType
{
    Create = 0,
    Move = 1,
    Delete = 2
}

public class MIKEWaypointService : MIKEService
{
    // Start is called before the first frame update
    void Start()
    {
        Service = ServiceType.Waypoint;
        IsReliable = true;
        MIKEInputManager.Main.RegisterService(Service, this);
    }

    public override void ReceiveData(MIKEPacket packet)
    {
        // Parse waypoint action
        int serviceType = packet.ReadInt();

        // Parse waypoint ID
        int waypointID = packet.ReadInt();

        if (serviceType == (int)WaypointServiceType.Create)
        {
            // Parse data
            float xPos = packet.ReadFloat();
            float yPos = packet.ReadFloat();
            int waypointNum = packet.ReadInt();

            Vector3 waypointPos = MIKEMap.Main.GetPositionFromNormalized(new Vector2(xPos, yPos));
            MIKENotificationManager.Main.SendNotification("NOTIFICATION", "Waypoint received from HUD", MIKEResources.Main.PositiveNotificationColor, 5f);
            LMCCWaypointSpawner.Main.SpawnNewHUDWaypoint(waypointID, waypointNum, waypointPos);
        }
        else
        {
            Debug.LogError("MIKEWaypointService: Invalid waypoint service type");
        }
    }
}
