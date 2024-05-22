using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIKEIntro : MonoBehaviour
{
    public bool Complete { get; private set; } = false;
    private Dissipate[] dissipate;

    public void Play()
    {
        dissipate = GetComponentsInChildren<Dissipate>();
        foreach (Dissipate d in dissipate)
        {
            d.StartDissipate(true, 3f);
        }
        StartCoroutine(ShowApp());
        MIKESFXManager.main.PlaySFX("Welcome", 1);
    }

    public IEnumerator ShowApp()
    {
        yield return new WaitForSeconds(3.5f);
        Complete = true;
    }
}
