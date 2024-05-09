using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObject : MonoBehaviour
{
    public Transform Target { get => target; set => target = value; }

    [SerializeField] private Transform target;
    [SerializeField] private bool ignoreY;
    [SerializeField] private bool invertDirection;

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = transform.position - target.position;
        if (ignoreY) forward = Vector3.ProjectOnPlane(forward, Vector3.up);
        transform.forward = forward * (invertDirection ? -1f : 1f);
    }
}
