using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Velocity : MonoBehaviour
{

    private Queue<Vector3> velocities = new Queue<Vector3>();
    private Vector3 posLastFrame;

    void Update()
    {
        // Hand velocity

        Vector3 vel = (transform.position - posLastFrame) / Time.deltaTime;
        velocities.Enqueue(vel);

        if(velocities.Count > 2)
        {
            velocities.Dequeue();
        }

        posLastFrame = transform.position;

    }

    public Vector3 GetVelocity()
    {
        
        Vector3 avg = Vector3.zero;
        foreach(Vector3 vel in velocities)
        {
            avg += vel;
        }

        avg /= velocities.Count;

        return avg;

    }
}
