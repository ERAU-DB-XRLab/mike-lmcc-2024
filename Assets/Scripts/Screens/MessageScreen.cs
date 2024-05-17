using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageScreen : LMCCScreen
{
    [SerializeField] private Transform hudMessageParent;
    [SerializeField] private GameObject hudMessagePrefab;
    [SerializeField] private MenuButton micButton;

    private Dictionary<string, AudioClip> messages = new Dictionary<string, AudioClip>();
    private List<MIKEHUDMessage> hudMessages = new List<MIKEHUDMessage>();
    private bool recording = false;
    private float timer = 0;

    void OnEnable()
    {
        DeleteAllMessages();
        Debug.Log("Messages: " + messages.Count);

        foreach (KeyValuePair<string, AudioClip> pair in messages)
        {
            CreateNewMessage(pair.Value, pair.Key);
        }
    }

    public override void ScreenDeactivated()
    {
        if (micButton.Pressed)
        {
            micButton.PointerEnter(null);
            micButton.PointerExit(null);
        }

        DeleteAllMessages();
    }

    public void LoadClip(AudioClip clip)
    {
        string time = DateTime.Now.ToLongTimeString();
        messages.Add(time, clip);

        if (gameObject.activeSelf)
        {
            Debug.Log("Creating new message");
            CreateNewMessage(clip, time);
        }
    }

    private void CreateNewMessage(AudioClip clip, string time)
    {
        MIKEHUDMessage newMessage = Instantiate(hudMessagePrefab, hudMessageParent).GetComponent<MIKEHUDMessage>();
        hudMessages.Add(newMessage);
        newMessage.LoadClip(clip, time);
    }

    private void DeleteAllMessages()
    {
        foreach (MIKEHUDMessage message in hudMessages)
        {
            Destroy(message.gameObject);
        }
        hudMessages.Clear();
    }

    public void ToggleRecording()
    {
        if (!recording)
        {
            MIKEAudioTransmitter.Main.StartRecording();
        }
        else
        {
            MIKEAudioTransmitter.Main.StopRecording();
        }

        recording = !recording;
    }

    // Update is called once per frame
    void Update()
    {
        if (recording && !MIKEAudioTransmitter.Main.Recording)
        {
            recording = false;
            micButton.PointerEnter(null);
            micButton.PointerExit(null);
        }
    }
}
