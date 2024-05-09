using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TogglePassthrough : MonoBehaviour
{

    private ARCameraManager manager;

    void Awake()
    {
        manager = FindObjectOfType<ARCameraManager>();
    }

    public void Toggle()
    {
        if(!manager)
        {
            manager = FindObjectOfType<ARCameraManager>();
        }
        manager.enabled = !manager.enabled;
    }
}
