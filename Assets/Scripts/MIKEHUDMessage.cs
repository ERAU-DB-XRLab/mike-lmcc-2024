using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MIKEHUDMessage : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private Image fill;
    [SerializeField] private TextMeshProUGUI text;

    private bool toggled = false;

    public void LoadClip(AudioClip clip, string time)
    {
        source.clip = clip;
        text.SetText("LMCC Message - " + time);
    }

    public void ToggleClip()
    {
        if (source.isPlaying && toggled)
        {
            StopClip();
        }
        else if (!toggled)
        {
            PlayClip();
        }
        else
        {
            toggled = false;
        }
    }

    public void PlayClip()
    {
        source.Play();
        toggled = true;
    }

    public void StopClip()
    {
        source.Stop();
        toggled = false;
    }

    void Update()
    {
        if (source.isPlaying)
        {
            fill.fillAmount = source.time / source.clip.length;
        }
        else
        {
            fill.fillAmount = 1;
        }
    }
}
