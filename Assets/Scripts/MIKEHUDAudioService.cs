using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MIKEHUDAudioService : MIKEService
{

    // Reference data
    [SerializeField] private MessageScreen hudScreen;

    // Packet data
    private bool parsing = false;
    private int packetsParsed = 0;
    private List<byte> mainData = new List<byte>();
    private float firstPacketTime;


    void Start()
    {
        serviceID = 1;
        MIKEInputManager.Main.RegisterService(serviceID, this);
    }

    public override void ReceiveData(byte[] data)
    {

        List<byte> dataAsList = data.ToList();

        // remove device ID byte
        dataAsList.RemoveAt(0);

        if (!parsing)
        {
            parsing = true;
            firstPacketTime = Time.realtimeSinceStartup;
        }

        mainData.AddRange(dataAsList);

        //if (packetsParsed >= packetCount)
        //{
        //    StoreClip(mainData.ToArray());
        //    ResetData();
        //}
    }

    void Update()
    {
        if (parsing)
        {
            if (Time.realtimeSinceStartup - firstPacketTime > 1f)
            {
                // Cutoff
                StoreClip(mainData.ToArray());
                ResetData();
            }
        }
    }

    public void ResetData()
    {
        mainData.Clear();
        packetsParsed = 0;
        parsing = false;
    }

    public void StoreClip(byte[] data)
    {

        Debug.Log("clip stored!");
        float[] floatArray = new float[Mathf.CeilToInt(data.Length / 4f)];
        Buffer.BlockCopy(data, 0, floatArray, 0, data.Length);

        AudioClip clip = AudioClip.Create("Test", floatArray.Length, 1, 16000, false);
        clip.SetData(floatArray, 0);

        hudScreen.CreateNewMessage(clip);

        MIKENotificationManager.Main.SendNotification("NOTIFICATION", "New LMCC Message", MIKEResources.Main.PositiveNotificationColor, 2.5f);
    }

}
