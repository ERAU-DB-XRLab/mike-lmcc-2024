using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AstronautScreen : LMCCScreen
{
    [SerializeField] private MIKEWidgetValue astroX;
    [SerializeField] private MIKEWidgetValue astroY;
    [SerializeField] private MIKEWidgetValue heading;
    [Space]
    [SerializeField] private MIKEWidgetValue otherX;
    [SerializeField] private MIKEWidgetValue otherY;
    [SerializeField] private MIKEWidgetValue otherHeading;
    [Space]
    [SerializeField] private RawImage cameraFeed;

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

        otherX.SetValue((float)data.OtherEVA.posx, MIKEResources.Main.PositiveNotificationColor);
        otherY.SetValue((float)data.OtherEVA.posy, MIKEResources.Main.PositiveNotificationColor);
        otherHeading.SetValue((float)data.OtherEVA.heading, MIKEResources.Main.PositiveNotificationColor);
    }

    public void UpdateCameraFeed(Texture2D texture)
    {
        cameraFeed.texture = texture;
    }
}
