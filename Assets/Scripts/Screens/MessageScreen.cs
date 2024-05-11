using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageScreen : LMCCScreen
{
    [SerializeField] private Transform hudMessageParent;
    [SerializeField] private GameObject hudMessagePrefab;
    [SerializeField] private MenuButton micButton;

    public void CreateNewMessage(AudioClip clip)
    {
        MIKEHUDMessage newMessage = Instantiate(hudMessagePrefab, hudMessageParent).GetComponent<MIKEHUDMessage>();
        newMessage.LoadClip(clip);
    }

    public override void ScreenDeactivated()
    {
        if (micButton.Pressed)
        {
            micButton.PointerEnter(null);
            micButton.PointerExit(null);
        }
    }
}
