using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MIKEIntro intro;
    [SerializeField] private LMCCCalibration calibration;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartApplication());
    }

    private IEnumerator StartApplication()
    {
        intro.Play();
        yield return new WaitUntil(() => intro.Complete);
        calibration.Calibrate();
    }
}
