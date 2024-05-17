using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIKESpecManager : MonoBehaviour
{
    public static MIKESpecManager Main { get; private set; }

    public List<SpecData> SpecData { get; private set; }

    void Awake()
    {
        if (Main == null)
            Main = this;
        else
            Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        SpecData = new List<SpecData>();
        TSSManager.Main.OnSpecUpdated += UpdateSpec;
    }

    private void UpdateSpec(SpecData data)
    {
        if (data.id > 0)
        {
            SpecData.Add(data);
            MIKENotificationManager.Main.SendNotification("NOTIFICATION", "New specimen data has been received!", MIKEResources.Main.PositiveNotificationColor, 5f);
        }
    }
}
