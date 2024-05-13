using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstronautScreen : LMCCScreen
{
    [SerializeField] private MIKEWidgetValue astroX;
    [SerializeField] private MIKEWidgetValue astroY;
    [SerializeField] private MIKEWidgetValue heading;

    // Start is called before the first frame update
    void Start()
    {
        TSSManager.Main.OnIMUUpdated += UpdateIMU;
    }

    void OnEnable()
    {
        UpdateIMU(TSSManager.Main.IMUData);
    }

    private void UpdateIMU(IMUData data)
    {
        astroX.SetValue((float)data.YourEVA.posx, MIKEResources.Main.PositiveNotificationColor);
        astroY.SetValue((float)data.YourEVA.posy, MIKEResources.Main.PositiveNotificationColor);
        heading.SetValue((float)data.YourEVA.heading, MIKEResources.Main.PositiveNotificationColor);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
