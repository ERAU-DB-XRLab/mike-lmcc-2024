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
        LMCCNavBar.Main.Display(false, () =>
        {
            LMCCNavBar.Main.Buttons.ForEach(button => button.gameObject.SetActive(false));
        });
        currWaypoint.Grabbed = true;
        currWaypoint.Placed = false;
    }

    private void DroppedWaypoint(MenuComponent component, HandInteract interact)
    {
        if (!currWaypoint.Placed)
        {
            Destroy(currWaypoint.gameObject);
        }

        SpawnNewWaypoint();
        LMCCNavBar.Main.Buttons.ForEach(button => button.gameObject.SetActive(true));
        LMCCNavBar.Main.Display(true);
    }

    public void SpawnNewWaypoint()
    {
        currWaypoint = Instantiate(waypointPrefab, this.transform).GetComponent<LMCCWaypoint>();
        currWaypoint.GetComponent<MenuComponent>().OnUIGrabbed += GrabbedWaypoint;
        currWaypoint.GetComponent<MenuComponent>().OnUIDropped += DroppedWaypoint;
    }

    public void SpawnNewHUDWaypoint(Vector3 localPosition)
    {
        currWaypoint = Instantiate(waypointPrefab, MIKEMap.Main.transform.parent).GetComponent<LMCCWaypoint>();
        currWaypoint.transform.localPosition = localPosition;
        currWaypoint.GetComponent<MenuComponent>().OnUIGrabbed += GrabbedWaypoint;
        currWaypoint.GetComponent<MenuComponent>().OnUIDropped += DroppedWaypoint;
    }
}
