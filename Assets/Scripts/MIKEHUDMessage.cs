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

    public void LoadClip(AudioClip clip)
    {
        source.clip = clip;
        text.SetText("LMCC Message - " + DateTime.Now.ToLongTimeString());
    }

    public void PlayClip()
    {
        source.Play();
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
