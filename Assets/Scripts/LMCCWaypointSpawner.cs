using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMCCWaypointSpawner : MonoBehaviour
{
    public static LMCCWaypointSpawner Main { get; private set; }

    [SerializeField] private GameObject waypointPrefab;
    [SerializeField] private GameObject hudWaypointPrefab;

    private LMCCWaypoint currWaypoint;
    public int LMCCWaypointCount { get; set; } = 0;

    void Awake()
    {
        if (Main == null)
            Main = this;
        else
            Destroy(this);
    }


    // Start is called before the first frame update
    void Start()
    {
        SpawnNewWaypoint();
    }

    private void GrabbedWaypoint(MenuComponent component, HandInteract interact)
    {
        LMCCNavBar.Main.Hide();
    }

    private void DroppedWaypoint(MenuComponent component, HandInteract interact)
    {
        currWaypoint.GetComponent<MenuComponent>().OnUIGrabbed -= GrabbedWaypoint;
        currWaypoint.GetComponent<MenuComponent>().OnUIDropped -= DroppedWaypoint;

        SpawnNewWaypoint();
        LMCCNavBar.Main.Show();
    }

    public void SpawnNewWaypoint()
    {
        currWaypoint = Instantiate(waypointPrefab, this.transform).GetComponent<LMCCWaypoint>();
        currWaypoint.GetComponent<MenuComponent>().OnUIGrabbed += GrabbedWaypoint;
        currWaypoint.GetComponent<MenuComponent>().OnUIDropped += DroppedWaypoint;
    }

    // Spawn a new HUD waypoint at a specific position
    public void SpawnNewHUDWaypoint(int waypointID, int waypointNum, Vector3 position)
    {
        LMCCWaypoint hudWaypoint = Instantiate(hudWaypointPrefab, this.transform).GetComponent<LMCCWaypoint>();
        hudWaypoint.transform.SetParent(MIKEMap.Main.transform);
        hudWaypoint.WaypointID = waypointID;
        hudWaypoint.WaypointNumber.SetValue(waypointNum.ToString());
        hudWaypoint.transform.position = position;

        if (MIKEMap.Main.IsPositionOnMap(hudWaypoint.transform.position, out RaycastHit hit))
        {
            hudWaypoint.Ring.transform.rotation = Quaternion.LookRotation(hit.normal);
        }

        hudWaypoint.GetComponentInChildren<HoverAbove>().IsBouncing = true;
        hudWaypoint.GetComponentInChildren<Rotate>().IsRotating = true;
        hudWaypoint.Placed = true;
    }
}
