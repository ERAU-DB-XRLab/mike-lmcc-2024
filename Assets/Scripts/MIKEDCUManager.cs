using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIKEDCUManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (TSSManager.Main.DCUData.batt)
        {
            MIKENotificationManager.Main.SendNotification("DCU", "Battery Low", MIKEResources.Main.NegativeNotificationColor, 5f);
        }

        if (TSSManager.Main.DCUData.oxy)
        {
            MIKENotificationManager.Main.SendNotification("DCU", "Oxygen Low", MIKEResources.Main.NegativeNotificationColor, 5f);
        }

        if (TSSManager.Main.DCUData.comm)
        {
            MIKENotificationManager.Main.SendNotification("DCU", "Comm Low", MIKEResources.Main.NegativeNotificationColor, 5f);
        }
    }
}
