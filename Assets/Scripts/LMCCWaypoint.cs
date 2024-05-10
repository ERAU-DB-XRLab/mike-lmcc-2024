using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMCCWaypoint : MonoBehaviour
{
    public bool Placed { get; set; } = false;
    public bool Grabbed { get; set; } = false;

    [SerializeField] private LayerMask mapLayer;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform icon;
    [Space]
    [SerializeField] private float maxRayDistance = 0.2f;
    [SerializeField] private float lineWidth = 0.002f;
    [SerializeField] private float iconOffset = 0.002f;

    private RaycastHit hit;

    private bool hitMap = false;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.enabled = false;
        icon.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Grabbed)
        {
            Debug.Log("Grabbed");
            Debug.DrawRay(transform.position, Vector3.down * maxRayDistance, Color.red);
            if (Physics.Raycast(transform.position, Vector3.down, out hit, maxRayDistance, mapLayer))
            {
                if (!hitMap)
                {
                    hitMap = true;
                    GetComponent<MenuComponent>().OnUIDropped += DroppedWaypoint;
                    Placed = true;
                    EnableHitMarkers();
                }

                lineRenderer.SetPositions(new Vector3[] { lineRenderer.gameObject.transform.position, hit.point });
                lineRenderer.startWidth = lineWidth;
                lineRenderer.endWidth = lineWidth;
                icon.SetPositionAndRotation(hit.point + hit.normal.normalized * iconOffset, Quaternion.LookRotation(hit.normal));
            }
            else
            {
                if (hitMap)
                {
                    hitMap = false;
                    GetComponent<MenuComponent>().OnUIDropped -= DroppedWaypoint;
                    Placed = false;
                    DisableHitMarkers();
                }
            }
        }
    }

    public void DroppedWaypoint(MenuComponent component, HandInteract interact)
    {
        Debug.Log("Dropped");
        DisableHitMarkers();
        Grabbed = false;
        transform.SetParent(MIKEMap.Main.transform.parent);
        transform.position = hit.point;

        // SEND IT TO THE HUD
        Vector2 normalizedPos = MIKEMap.Main.NormalizePosition(transform.localPosition);
        List<byte> byteList = new List<byte>();
        byteList.AddRange(BitConverter.GetBytes(normalizedPos.x));
        byteList.AddRange(BitConverter.GetBytes(normalizedPos.y));
        MIKEServerManager.Main.SendDataReliably(ServiceType.Waypoint, byteList.ToArray());
    }

    private void EnableHitMarkers()
    {
        lineRenderer.enabled = true;
        icon.gameObject.SetActive(true);
    }

    private void DisableHitMarkers()
    {
        lineRenderer.enabled = false;
        icon.gameObject.SetActive(false);
    }
}
