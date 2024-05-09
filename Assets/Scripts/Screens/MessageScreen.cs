using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageScreen : LMCCScreen
{
    [SerializeField] private Transform hudMessageParent;
    [SerializeField] private GameObject hudMessagePrefab;

    public void CreateNewMessage(AudioClip clip)
    {
        MIKEHUDMessage newMessage = Instantiate(hudMessagePrefab, hudMessageParent).GetComponent<MIKEHUDMessage>();
        newMessage.LoadClip(clip);
    }
}
