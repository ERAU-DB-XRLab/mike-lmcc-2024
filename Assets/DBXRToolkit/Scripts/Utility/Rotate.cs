using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public bool IsRotating { get => rotate; set => rotate = value; }

    public Vector3 rotation;

    [SerializeField] private bool rotate = true;

    // Update is called once per frame
    void Update()
    {
        if (rotate)
            transform.Rotate(rotation * Time.deltaTime);
    }
}
