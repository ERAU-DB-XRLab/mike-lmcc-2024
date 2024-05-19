using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MIKEAudioTransmitter : MonoBehaviour
{
    public static MIKEAudioTransmitter Main { get; private set; }

    private AudioClip currentClip;
    private float[] audioData;
    private float timer;
    private bool recording = false;

    public bool Recording { get => recording; }
    public int MaxSeconds { get => maxSeconds; }
    public int Frequency { get => frequency; }

    [SerializeField] private int maxSeconds = 15;
    [SerializeField] private int frequency = 16000;

    [Header("DEBUG")]
    [SerializeField] private bool debugMode = false;
    [SerializeField] private string deviceName;
    [SerializeField] private KeyCode startKey = KeyCode.M;

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
        if (debugMode)
        {
            foreach (var device in Microphone.devices)
            {
                Debug.Log("Audio Device: " + device);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (recording)
        {
            timer += Time.deltaTime;

            if (timer >= maxSeconds)
            {
                StopRecording();
            }
        }

        if (debugMode)
        {
            if (Input.GetKeyDown(startKey))
            {
                StartRecording();
            }

            if (Input.GetKeyUp(startKey))
            {
                StopRecording();
            }
        }
    }

    public void StartRecording()
    {
        currentClip = Microphone.Start(debugMode ? deviceName : Microphone.devices[0], false, maxSeconds, frequency);
        timer = 0;
        recording = true;
    }

    public void StopRecording()
    {
        recording = false;
        Microphone.End(debugMode ? deviceName : Microphone.devices[0]);
        audioData = new float[currentClip.samples * currentClip.channels];
        currentClip.GetData(audioData, 0);
        audioData = audioData.ToList().GetRange(0, (int)(frequency * Mathf.Min(timer, maxSeconds))).ToArray();

        var packet = new MIKEPacket();
        foreach (var sample in audioData)
        {
            packet.Write(sample);
        }

        MIKEServerManager.Main.SendData(ServiceType.Audio, packet, DeliveryType.Unreliable);

        MIKENotificationManager.Main.SendNotification("NOTIFICATION", "Audio message sent", MIKEResources.Main.PositiveNotificationColor, 5f);
    }

    public void SendData()
    {
        byte[] byteArray = new byte[audioData.Length * 4];
        Buffer.BlockCopy(audioData, 0, byteArray, 0, byteArray.Length);
        Debug.Log("Sending sample count: " + audioData.Length);
        var packet = new MIKEPacket(byteArray);
        MIKEServerManager.Main.SendData(ServiceType.Audio, packet, DeliveryType.Unreliable);
    }
}
