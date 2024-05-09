using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandMovementTeleport : MonoBehaviour
{

    [Header("Player Movement Method (Pick One)")]
    [SerializeField] private Transform player;

    [Space]
    [SerializeField] private Transform teleportDirection;
    [SerializeField] private LineRenderer r;
    [SerializeField] private float teleportDistance, lineGravity;
    [SerializeField] private LayerMask groundLayerMask;

    [Space]
    [SerializeField] private InputActionProperty move;

    private Vector3 teleportPos;

    // Update is called once per frame
    void Update()
    {

        Vector2 input = move.action.ReadValue<Vector2>();
        if(input.y > 0.7f)
        {
            List<Vector3> linePositions = GetLinePositions();
            if (linePositions != null)
            {
                r.positionCount = linePositions.Count;
                r.SetPositions(linePositions.ToArray());
            }
            else
            {
                r.positionCount = 0;
            }
        } else
        {
            if(r.positionCount > 0)
            {
                // Teleport player
                player.position = r.GetPosition(r.positionCount - 1);
                r.positionCount = 0;
            }
        }

    }

    public List<Vector3> GetLinePositions()
    {

        teleportPos = Vector3.zero;

        List<Vector3> positions = new List<Vector3>();
        Vector3 pos = teleportDirection.position;
        Vector3 vel = teleportDirection.forward * teleportDistance;

        float totalTime = 5f;
        float stepSize = 0.0125f; // measured in seconds

        for(float i = 0; i < totalTime; i+= stepSize)
        {
            positions.Add(pos);

            Ray r = new Ray(pos, vel);
            RaycastHit hit;
            if(Physics.Raycast(r, out hit, ((pos + (vel * stepSize)) - pos).magnitude, groundLayerMask))
            {
                teleportPos = hit.point;
                positions.Add(hit.point);
                break;
            }

            // Calculate next position
            pos += vel * stepSize;
            vel -= new Vector3(0, lineGravity * stepSize, 0);

        }

        if(teleportPos == Vector3.zero)
        {
            return null;
        }

        return positions;

    }

}
