using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MIKEMicrophoneService : MonoBehaviour
{
    [SerializeField] private string testMic = "Microphone (Yeti Stereo Microphone)";

    private AudioSource source;
    private AudioClip currentClip;
    private float[] audioData;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        foreach (string device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // FOR DEBUG
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentClip = Microphone.Start(testMic, false, 30, 16000);
            timer = 0;
            source.clip = currentClip;
            source.loop = false;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Microphone.End(testMic);
            audioData = new float[currentClip.samples * currentClip.channels];
            currentClip.GetData(audioData, 0);

            List<float> editedData = audioData.ToList().GetRange(0, (int)(16000f * timer));
            audioData = editedData.ToArray();

            SendData();
        }

        timer += Time.deltaTime;

    }

    public void BeginRecording()
    {
        currentClip = Microphone.Start(testMic, false, 30, 16000);
        timer = 0;
        source.clip = currentClip;
        source.loop = false;
    }

    public void EndRecording()
    {
        Microphone.End(testMic);
        audioData = new float[currentClip.samples * currentClip.channels];
        currentClip.GetData(audioData, 0);

        List<float> editedData = audioData.ToList().GetRange(0, (int)(16000f * timer));
        audioData = editedData.ToArray();

        SendData();
    }

    private void SendData()
    {
        byte[] byteArray = new byte[audioData.Length * 4];
        Buffer.BlockCopy(audioData, 0, byteArray, 0, byteArray.Length);
        Debug.Log("Sending sample count: " + audioData.Length);
        MIKEServerManager.Main.SendData(ServiceType.Audio, byteArray);
    }

}
