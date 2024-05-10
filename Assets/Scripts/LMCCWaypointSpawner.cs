using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMCCWaypointSpawner : MonoBehaviour
{
    public static LMCCWaypointSpawner Main { get; private set; }

    [SerializeField] private GameObject waypointPrefab;

    private LMCCWaypoint currWaypoint;

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
    public void SpawnNewHUDWaypoint(int waypointID, Vector3 position)
    {
        LMCCWaypoint hudWaypoint = Instantiate(waypointPrefab, MIKEMap.Main.transform).GetComponent<LMCCWaypoint>();
        hudWaypoint.WaypointID = waypointID;
        hudWaypoint.transform.position = position;

        if (Physics.Raycast(hudWaypoint.transform.position + Vector3.up * 500, Vector3.down, out RaycastHit hit, 1000, LayerMask.GetMask("Map")))
        {
            hudWaypoint.Ring.transform.rotation = Quaternion.LookRotation(hit.normal);
        }

        hudWaypoint.Placed = true;
    }
}
