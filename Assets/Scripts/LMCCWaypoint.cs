using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMCCWaypoint : MonoBehaviour
{
    public bool Placed { get; set; } = false;
    public bool Grabbed { get; set; } = false;
    public bool Hovering { get; set; } = false;
    public int WaypointID { get; set; } = -1;

    public Transform Ring { get => ring; }

    [SerializeField] private LayerMask mapLayer;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform icon;
    [SerializeField] private Transform ring;
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

        GetComponent<MenuComponent>().OnUIGrabbed += (component, interact) =>
        {
            // If the waypoint was already placed and is getting regrabbed
            if (Placed)
            {
                LMCCNavBar.Main.Hide();
                LMCCWaypointSpawner.Main.gameObject.SetActive(false);
            }

            Grabbed = true;
            ResetWaypoint();
        };

        GetComponent<MenuComponent>().OnUIDropped += (component, interact) =>
        {
            if (Placed)
            {
                LMCCNavBar.Main.Show();
                LMCCWaypointSpawner.Main.gameObject.SetActive(true);
            }

            SendToHUD();

            if (!Hovering)
            {
                Destroy(gameObject);
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Grabbed)
        {
            Debug.DrawRay(transform.position, Vector3.down * maxRayDistance, Color.red);
            if (Physics.Raycast(transform.position, Vector3.down, out hit, maxRayDistance, mapLayer))
            {
                if (!hitMap)
                {
                    hitMap = true;
                    GetComponent<MenuComponent>().OnUIDropped += DropWaypoint;
                    Hovering = true;
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
                    GetComponent<MenuComponent>().OnUIDropped -= DropWaypoint;
                    Hovering = false;
                    DisableHitMarkers();
                }
            }
        }
    }

    public void DropWaypoint(MenuComponent component, HandInteract interact)
    {
        GetComponent<MenuComponent>().OnUIDropped -= DropWaypoint;
        DisableHitMarkers();

        Grabbed = false;
        Hovering = false;
        hitMap = false;

        transform.SetParent(MIKEMap.Main.transform);
        transform.position = hit.point;
        ring.transform.rotation = Quaternion.LookRotation(hit.normal);
        GetComponentInChildren<HoverAbove>().IsBouncing = true;
        GetComponentInChildren<Rotate>().IsRotating = true;
        if (!Placed)
        {
            WaypointID = UnityEngine.Random.Range(0, 10000000);
        }

        // SEND IT TO THE HUD
        SendToHUD();

        Placed = true;
    }

    public void SendToHUD()
    {
        List<byte> byteList = new List<byte>();
        byteList.AddRange(BitConverter.GetBytes(WaypointID));

        if (Placed && !Hovering)
        {
            byteList.Add((byte)'D'); // Existing waypoint was deleted
        }
        else
        {
            Vector2 normalizedPos = MIKEMap.Main.NormalizePosition(transform.localPosition);

            if (Placed)
            {
                byteList.Add((byte)'M'); // Existing waypoint was moved
            }
            else
            {
                byteList.Add((byte)'C'); // New waypoint was created
            }

            byteList.AddRange(BitConverter.GetBytes(normalizedPos.x));
            byteList.AddRange(BitConverter.GetBytes(normalizedPos.y));
        }

        MIKEServerManager.Main.SendDataReliably(ServiceType.Waypoint, byteList.ToArray());
    }

    private void ResetWaypoint()
    {
        transform.SetParent(LMCCMenuSpawner.Main.gameObject.transform);
        GetComponentInChildren<HoverAbove>().IsBouncing = false;
        GetComponentInChildren<Rotate>().IsRotating = false;
        ring.transform.rotation = Quaternion.Euler(-90, 0, 0);
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
