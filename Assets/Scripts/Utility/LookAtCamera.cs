using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{

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
        transform.forward = (transform.position - cameraPos) * (invertDirection ? -1f : 1f);
    }
}
