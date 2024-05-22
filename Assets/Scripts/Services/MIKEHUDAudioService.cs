using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MIKEHUDAudioService : MIKEService
{

    // Start is called before the first frame update
    void Start()
    {
        Service = ServiceType.Audio;
        IsReliable = false;
        MIKEInputManager.Main.RegisterService(Service, this);
    }

    public override void ReceiveData(MIKEPacket packet)
    {
        Debug.Log("Audio received");
        float[] samples = new float[Mathf.CeilToInt(packet.UnreadByteArray.Length / 4f)];
        for (int i = 0; i < samples.Length; i++)
        {
            samples[i] = packet.ReadFloat();
        }

        AudioClip clip = AudioClip.Create("Message", samples.Length, 1, MIKEAudioTransmitter.Main.Frequency, false);
        clip.SetData(samples, 0);
        ((MessageScreen)LMCCMenuSpawner.Main.Menus[(int)ScreenType.Message].CurrentScreen).LoadClip(clip);
        MIKENotificationManager.Main.SendNotification("NOTIFICATION", "New message received from the HUD", MIKEResources.Main.PositiveNotificationColor, 5f);
    }
}
