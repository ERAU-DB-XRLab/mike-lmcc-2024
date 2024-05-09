using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TogglePassthrough : MonoBehaviour
{

    [SerializeField] private ARCameraManager manager;
    [SerializeField] private Camera c;

    public void Toggle()
    {
        manager.enabled = !manager.enabled;
        //if(!manager.enabled)
        //{
        //    c.backgroundColor = new Color(0, 0, 0, 1);
        //} else
        //{
        //    c.backgroundColor = new Color(0, 0, 0, 0);
        //}
    }
}
