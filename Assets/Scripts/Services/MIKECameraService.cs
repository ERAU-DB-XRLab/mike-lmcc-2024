using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MIKECameraService : MIKEService
{
    public Texture2D Frame { get; private set; }

    [SerializeField] private int width = 640;
    [SerializeField] private int height = 480;

    // Start is called before the first frame update
    void Start()
    {
        Service = ServiceType.Camera;
        IsReliable = false;
        MIKEInputManager.Main.RegisterService(Service, this);
    }

    public override void ReceiveData(MIKEPacket packet)
    {
        Frame = new Texture2D(width, height);
        Frame.LoadImage(packet.UnreadByteArray);
        Frame.Apply();

        if (LMCCMenuSpawner.Main.Menus[(int)ScreenType.Astronaut].IsActive)
        {
            ((AstronautScreen)LMCCMenuSpawner.Main.Menus[(int)ScreenType.Astronaut].CurrentScreen).UpdateCameraFeed(Frame);
        }
    }
}
