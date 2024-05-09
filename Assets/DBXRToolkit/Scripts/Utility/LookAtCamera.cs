using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool ignoreY;
    [SerializeField] private float yOffset;
    [SerializeField] private bool invertDirection;
    private Transform mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPos = mainCamera.position;
        cameraPos.y -= yOffset;
        Vector3 forward = transform.position - cameraPos;
        if (ignoreY) forward = Vector3.ProjectOnPlane(forward, Vector3.up);
        transform.forward = forward * (invertDirection ? -1f : 1f);
    }
}
